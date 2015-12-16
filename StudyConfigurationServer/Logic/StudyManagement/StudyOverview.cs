#region Using

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServer.Logic.StudyManagement
{
    /// <summary>
    /// Class responsible for retrieving info about a study and creating a studyOverview
    /// </summary>
    public class StudyOverview
    {
        private readonly StudyStorageManager _studyStorageManager;
        private TaskStorageManager _taskStorage;

        public StudyOverview(StudyStorageManager studyStorageManager, TaskStorageManager taskStorageManager)
        {
            _studyStorageManager = studyStorageManager;
            _taskStorage = taskStorageManager;
        }

        public StudyOverview()
        {
            _studyStorageManager = new StudyStorageManager();
            _taskStorage = new TaskStorageManager();
        }

        /// <summary>
        /// Return a studyOverviewDTO
        /// </summary>
        /// <param name="id"> study id</param>
        /// <returns></returns>
        public StudyOverviewDto GetOverview(int id)
        {
            var study = _studyStorageManager.GetAll()
                .Where(s => s.ID == id)
                .Include(st => st.Stages.Select(s => s.Tasks.Select(t => t.Users)))
                .FirstOrDefault();

            var studyOverview = new StudyOverviewDto
            {
                Name = study.Name,
                UserIds = GetUserIDs(study),
                Stages = GetStages(study)
            };
            return studyOverview;
        }

        /// <summary>
        /// Retrieve all userIds working on the study
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        private int[] GetUserIDs(Study study)
        {
            return study.Team.Users.Select(u => u.ID).ToArray();
        }

        /// <summary>
        /// Retrieve the current stage of the study
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        private Stage GetCurrentStage(Study study)
        {
            Stage currentStage = null;

            foreach (var stage in study.Stages)
            {
                if (stage.IsCurrentStage)
                {
                    currentStage = stage;
                    break;
                }
            }
            return currentStage;
        }

        /// <summary>
        /// Retrieve an array of stageOverviews
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        private StageOverviewDto[] GetStages(Study study)
        {
            var numbOfStages = study.Stages.Count();
            var stageOverview = new StageOverviewDto[numbOfStages];

            for (var i = 0; i < numbOfStages; i++)
            {
                stageOverview[i] = new StageOverviewDto();
                stageOverview[i].Name = study.Stages.ToArray()[i].Name;
                stageOverview[i].CompletedTasks = GetCompletedTasks(study.Stages.ToArray()[i]);
                stageOverview[i].IncompleteTasks = GetIncompleteTasks(study.Stages.ToArray()[i]);
            }
            return stageOverview;
        }

        /// <summary>
        /// Retrieve a dictionary of users and their amount of completed tasks in the given stage
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        private Dictionary<int, int> GetCompletedTasks(Stage stage)
        {
            var completedTasks = new ConcurrentDictionary<int, int>();

            foreach (var task in stage.Tasks)
            {
                foreach (var user in task.Users)
                {
                    if (task.IsFinished(user.ID))
                    {
                        completedTasks.AddOrUpdate(user.ID, 1, (id, count) => count + 1);
                    }
                }
            }
            return completedTasks.ToDictionary(k => k.Key, k => k.Value);
        }

        /// <summary>
        /// returns a dictionary of users and their incompleted tasks for the given stage
        /// </summary>
        /// <param name="stage"></param>
        /// <returns></returns>
        private Dictionary<int, int> GetIncompleteTasks(Stage stage)
        {
            var inCompletedTasks = new ConcurrentDictionary<int, int>();

            foreach (var task in stage.Tasks)
            {
                foreach (var user in task.Users)
                {
                    if (!task.IsFinished(user.ID))
                    {
                        inCompletedTasks.AddOrUpdate(user.ID, 1, (id, count) => count + 1);
                    }
                }
            }

            return inCompletedTasks.ToDictionary(k => k.Key, k => k.Value);
        }
    }
}