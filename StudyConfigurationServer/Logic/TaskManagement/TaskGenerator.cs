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
      
       
        public IEnumerable<StudyTask> GenerateTasks(Study study)
        {
           var currentStage = study.Stages.First(s => s.Id.Equals(study.CurrentStageID));

           foreach (var item in study.Items)
            {
                var task = new StudyTask()
                {
                    Paper = item,
                    TaskType = currentStage.StageType,
                    Stage = currentStage,
                };
                yield return task;
            }
        }

    }
}
