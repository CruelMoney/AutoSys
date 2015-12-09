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
                    DataFields = new List<DataField>()
                };

                foreach (var criterion in stage.Criteria)
                {
                    var dataField = new DataField()
                    {
                        Description = criterion.Description,
                        FieldType = criterion.DataType,
                        Name = criterion.Name,
                        UserData = new List<UserData>(),
                    };

                    if (criterion.DataType == DataField.DataType.Enumeration || criterion.DataType == DataField.DataType.Flags)
                    {
                        dataField.TypeInfo = criterion.TypeInfo;
                    }

                    task.DataFields.Add(dataField);
                }
               
                yield return task;
            }
        }

        public IEnumerable<StudyTask> GenerateValidateTasks(IEnumerable<StudyTask> conflictingTasks, Stage stage)
        {

            //HOWTO: somewhere else we have checked if the userData match and used the criteria to remove or keep the item.
            //1. Then we give this method all the conflicting tasks
            //2. It creates a new task with all the same fields just without userdata
            //3. then it adds the userdata from the conflicting task to the conflicting data array
            //4. this way we can check the final userData in this task and use critera hereon. The previous task will just have kept the item in the study
            throw new NotImplementedException();
           


                if (stage.StageType != StudyTask.Type.Conflict)
                {
                    throw new ArgumentException("The stage is not a conflict stage");
                }
         
            //For each conflicting task we add a new task
            foreach (var conflictTask in conflictingTasks)
            {
                var task = new StudyTask()
                {
                    Paper = conflictTask.Paper,
                    TaskType = stage.StageType,
                    Stage = stage,
                    DataFields = new List<DataField>()
                };

                var numberOfDatafields = conflictTask.DataFields.Count;
                
                foreach (var dataField in conflictTask.DataFields)
                {
                    task.DataFields.Add(new DataField()
                    {
                        Description = dataField.Description,
                        FieldType = dataField.FieldType,
                        Name = dataField.Name,
                        UserData = new List<UserData>(),
                    });

                }

                yield return task;
            }
        }

    }
}
