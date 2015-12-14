using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement
{
    public class TaskGenerator
    {
        public StudyTask GenerateReviewTask(Item item, List<Criteria> criteria)
        {
                var task = new StudyTask()
                {
                    Paper = item,
                    TaskType = StudyTask.Type.Review,
                    DataFields = new List<DataField>(),
                    IsEditable = true,
                    Users = new List<User>(),
                };

                foreach (var criterion in criteria)
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
                    foreach (var s in criterion.TypeInfo)
                    {
                       dataField.TypeInfo.Add(s);
                    }
               
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
                    DataFields = new List<DataField>(),
                    IsEditable = true,
                    Users = new List<User>(),
                    
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

