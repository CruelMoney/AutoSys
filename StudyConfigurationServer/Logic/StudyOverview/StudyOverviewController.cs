using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServer.Logic.StudyConfiguration;
using StudyConfigurationServer.Models;
using System.Collections.Concurrent;
using System.Data.Entity;

namespace StudyConfigurationServer.Logic.StudyOverview
{
    public class StudyOverviewController
    {
        private StudyStorageManager _studyStorageManager;
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

        public StudyOverviewDTO GetOverview(int id)
        {
            Study study = _studyStorageManager.GetAllStudies()
                .Where(s => s.ID == id)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

            StudyOverviewDTO studyOverview = new StudyOverviewDTO()
            {

                Name = study.Name,
                UserIds = GetUserIDs(study),
                Phases = GetStages(study)

            };
            return studyOverview;
            
        }

        public int[] GetUserIDs(Study study)
        {
            return study.Team.Users.Select(u=>u.ID).ToArray();    
        }


        public Stage GetCurrentStage(Study study)
        {
            Stage currentStage = null;
           
            foreach (var stage in study.Stages)
            {
                if (stage.ID == study.CurrentStage().ID)
                {
                    currentStage = stage;
                    break;
                }
            }
            return currentStage;
        }

        public StageOverviewDTO[] GetStages(Study study)
        {
            
            var numbOfStages = study.Stages.Count();
            var stageOverview = new StageOverviewDTO[numbOfStages];
      
            for(int i = 0; i < numbOfStages; i++)
            {
                stageOverview[i] = new StageOverviewDTO();
                stageOverview[i].Name = study.Stages.ToArray()[i].Name;
                stageOverview[i].CompletedTasks = GetCompletedTasks(study.Stages.ToArray()[i]);
                stageOverview[i].IncompleteTasks = GetIncompleteTasks(study.Stages.ToArray()[i]);
            }
            return stageOverview;
        }

        public Dictionary<int, int> GetCompletedTasks(Stage stage)
        {
            var completedTasks = new ConcurrentDictionary<int, int>();

            foreach(var task in stage.Tasks)
            {
                var loadedTask = _taskStorage.GetAllTasks()
                    .Where(t => t.ID == task.ID)
                    .Include(t=>t.Users).FirstOrDefault();

                foreach (var user in loadedTask.Users)
                {
                    if (loadedTask.IsFinished(user.ID))
                    {
                        completedTasks.AddOrUpdate(user.ID, 1, (id, count) => count + 1);
                    }
                }
            }           
            return completedTasks.ToDictionary(k=> k.Key, k=> k.Value);
        }

        public Dictionary<int, int> GetIncompleteTasks(Stage stage)
        {
            var inCompletedTasks = new ConcurrentDictionary<int, int>();

            foreach (var task in stage.Tasks)
            {
                var loadedTask = _taskStorage.GetAllTasks()
                    .Where(t => t.ID == task.ID)
                    .Include(t => t.Users).FirstOrDefault();

                foreach (var user in loadedTask.Users)
                {
                    if (!loadedTask.IsFinished(user.ID))
                    {
                        inCompletedTasks.AddOrUpdate(user.ID, 1, (id, count) => count + 1);
                    }
                }
            }

            return inCompletedTasks.ToDictionary(k => k.Key, k => k.Value);
        }


    }
}