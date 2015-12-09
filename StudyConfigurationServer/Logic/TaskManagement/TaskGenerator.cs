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
        public StudyTask GenerateReviewTask(Item item, Stage stage)
        {
            if (stage.CurrentTaskType!=StudyTask.Type.Review)
            {
                throw new ArgumentException("The stage is not a review stage");
            }

          
                var task = new StudyTask()
                {
                    Paper = item,
                    TaskType = stage.CurrentTaskType,
                    Stage = stage,
                    DataFields = new List<DataField>(),
                    IsEditable = true
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
               
                return task;
            
        }

        public StudyTask GenerateValidateTasks(StudyTask conflictingTask)
        {
                var task = new StudyTask()
                {
                    Paper = conflictingTask.Paper,
                    TaskType = StudyTask.Type.Conflict,
                    Stage = conflictingTask.Stage,
                    DataFields = new List<DataField>(),
                    IsEditable = true
                };

                foreach (var dataField in conflictingTask.DataFields)
                {
                    task.DataFields.Add(new DataField()
                    {
                        Description = dataField.Description,
                        FieldType = dataField.FieldType,
                        Name = dataField.Name,
                        UserData = new List<UserData>(),
                        ConflictingData = dataField.UserData
                    });

                }

                return task;
            }
        }

    }

