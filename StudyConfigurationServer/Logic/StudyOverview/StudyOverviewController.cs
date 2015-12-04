using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServer.Logic.StudyConfiguration;
using StudyConfigurationServer.Models;

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
            
        }

        public int[] GetUserIDs(Study study)
        {
            var users = study.Reviewers;
            users.AddRange(study.Validators);

            var userList = new int[users.Count];
            int index = 0;
            foreach(var user in users)
            {
                userList[index] = user.Id;
                index++;
            }
            return userList;     
        }

        public Stage GetCurrentStage(Study study)
        {
            Stage currentStage = null;
           
            foreach (var stage in study.Stages)
            {
                if (stage.Id == study.CurrentStage)
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
            var userIds = GetUserIDs(study);
            var stageOverview = new StageOverviewDTO[numbOfStages];
            var stages = new Stage[numbOfStages];
            
            foreach(var stage in study.Stages)
            {
                stages[index] = stage;
            }
            

            for(int i = 0; i < numbOfStages; i++)
            {
                stageOverview[i].Name = stages[i].Name;
                stageOverview[i].CompletedTasks = GetCompletedTasks(study);
            }


            var currentStage = GetCurrentStage(study);




            return stageOverview;
        }

        public Dictionary<int, int> GetCompletedTasks(Study study)
        {
            var completedTaks = new Dictionary<int, int>(); 
            foreach(var user in study.)
            {
                if (task.RequestedData.)
                {
                    
                }
            }
        }


    }
}