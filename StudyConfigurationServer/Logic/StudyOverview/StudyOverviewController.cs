using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServer.Logic.StudyConfiguration;
using StudyConfigurationServer.Models;
using System.Collections.Concurrent;

namespace StudyConfigurationServer.Logic.StudyOverview
{
    public class StudyOverviewController
    {
        private StudyManager _studyManager;

        public StudyOverviewController(StudyManager studyManager)
        {
            _studyManager = studyManager;
        }

        public StudyOverviewController()
        {
            _studyManager = new StudyManager();
        }

        public StudyOverviewDTO GetOverview(int id)
        {
            Study study = _studyManager.GetStudy(id);

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
            return study.Team.UserIDs;    
        }


        public Stage GetCurrentStage(Study study)
        {
            Stage currentStage = null;
           
            foreach (var stage in study.Stages)
            {
                if (stage.Id == study.CurrentStageID)
                {
                    currentStage = stage;
                    break;
                }
            }
            return currentStage;
        }

        public StageOverviewDTO[] GetStages(Study study)
        {
            int index = 0;
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
                foreach (var user in task.Users)
                {
                    if (task.IsFinished(user.Id))
                    {
                        completedTasks.AddOrUpdate(user.Id, 1, (id, count) => count + 1);
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
                foreach (var user in task.Users)
                {
                    if (!task.IsFinished(user.Id))
                    {
                        inCompletedTasks.AddOrUpdate(user.Id, 1, (id, count) => count + 1);
                    }
                }
            }

            return inCompletedTasks.ToDictionary(k => k.Key, k => k.Value);
        }


    }
}