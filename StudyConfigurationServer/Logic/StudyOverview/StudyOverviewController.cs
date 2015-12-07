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
            var NumbOfUsers = study.Team.UserI


            var userList = new int[NumbOfUsers];
            int index = 0;
            foreach(var user in study.Users)
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
            var stageOverview = new StageOverviewDTO[numbOfStages];
            var stages = new Stage[numbOfStages];
            
            
            foreach(var stage in study.Stages)
            {
                stages[index++] = stage;
            }
            

            for(int i = 0; i < numbOfStages; i++)
            {
                stageOverview[i].Name = stages[i].Name;
                stageOverview[i].CompletedTasks = GetCompletedTasks(stages[i]);
                stageOverview[i].IncompleteTasks = GetIncompleteTasks(stages[i]);
            }
            return stageOverview;
        }

        public Dictionary<int, int> GetCompletedTasks(Stage stage)
        {

            var completedTasks = new ConcurrentDictionary<int, int>();

            foreach(var task in stage.Tasks)
            {
                if (task.IsFinished)
                {
                    foreach(var user in task.RequestedData)
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
                if (!task.IsFinished)
            {
                    foreach (var user in task.RequestedData)
                {
                        inCompletedTasks.AddOrUpdate(user.Id, 1, (id, count) => count + 1);
                    }
                }
            }

            return inCompletedTasks.ToDictionary(k => k.Key, k => k.Value);
        }


    }
}