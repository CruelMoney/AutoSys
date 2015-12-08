using System;
using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.TaskManagement.TaskDistributor;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TaskManagement
{
    public class TaskGenerator
    {
      
       
        public IEnumerable<StudyTask> GenerateTasks(Stage stage, List<Item> items)
        {
            foreach (var item in items)
            {
                var task = new StudyTask()
                {
                    Paper = item,
                    TaskType = stage.StageType,
                    Stage = stage,
                };
                yield return task;
            }
        }

        public IEnumerable<StudyTask> GenerateReviewTasks(IEnumerable<Item> items, Stage stage)
        {
            if (stage.StageType!=StudyTask.Type.Review)
            {
                throw new ArgumentException("The stage is not a review stage");
            }

            foreach (var item in items)
            {
                var task = new StudyTask()
                {
                    Paper = item,
                    TaskType = stage.StageType,
                    Stage = stage,
                };
                

                yield return task;
            }
        }

        public IEnumerable<StudyTask> GenerateValidateTasks(IEnumerable<StudyTask> conflictingTasks, Stage stage)
        {
           
                if (stage.StageType != StudyTask.Type.Conflict)
                {
                    throw new ArgumentException("The stage is not a conflict stage");
                }

            foreach (var conflictTask in conflictingTasks)
            {
                var task = new StudyTask()
                {
                    Paper = conflictTask.Paper,
                    TaskType = stage.StageType,
                    Stage = stage,
                };


                yield return task;
            }
        }

    }
}
