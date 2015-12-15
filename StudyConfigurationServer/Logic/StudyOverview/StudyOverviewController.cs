#region Using

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServer.Logic.StudyOverview
{
    public class StudyOverviewController
    {
        private readonly StudyStorageManager _studyStorageManager;
        private TaskStorageManager _taskStorage;

        public StudyOverviewController(StudyStorageManager studyStorageManager, TaskStorageManager taskStorageManager)
        {
            _studyStorageManager = studyStorageManager;
            _taskStorage = taskStorageManager;
        }

        public StudyOverviewController()
        {
            _studyStorageManager = new StudyStorageManager();
            _taskStorage = new TaskStorageManager();
        }

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
                Phases = GetStages(study)
            };
            return studyOverview;
        }

        public int[] GetUserIDs(Study study)
        {
            return study.Team.Users.Select(u => u.ID).ToArray();
        }


        public Stage GetCurrentStage(Study study)
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

        public StageOverviewDto[] GetStages(Study study)
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

        public Dictionary<int, int> GetCompletedTasks(Stage stage)
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

        public Dictionary<int, int> GetIncompleteTasks(Stage stage)
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