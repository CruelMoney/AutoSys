﻿#region Using

using System.Collections.Generic;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement
{
    public class TaskGenerator
    {
        public StudyTask GenerateReviewTask(Item item, List<Criteria> criteria)
        {
            var task = new StudyTask
            {
                Paper = item,
                TaskType = StudyTask.Type.Review,
                DataFields = new List<DataField>(),
                Users = new List<User>()
            };

            foreach (var criterion in criteria)
            {
                //Trying to parse data from the bib item
                var data = new StoredString {Value = item.FindFieldValue(criterion.Name)};

                var dataField = new DataField
                {
                    Description = criterion.Description,
                    FieldType = criterion.DataType,
                    Name = criterion.Name,
                    UserData = new List<UserData> {new UserData {Data = new List<StoredString> {data}}}
                };

                if (criterion.DataType == DataField.DataType.Enumeration ||
                    criterion.DataType == DataField.DataType.Flags)
                {
                    dataField.TypeInfo = new List<StoredString>();
                    foreach (var s in criterion.TypeInfo)
                    {
                        dataField.TypeInfo.Add(s);
                    }
                }

                //Check if the information was found in the bib item, if yes the task is finished. 
                task.IsEditable = data.Value == null;

                task.DataFields.Add(dataField);
            }

            return task;
        }

        public StudyTask GenerateValidateTasks(StudyTask conflictingTask)
        {
            var task = new StudyTask
            {
                Paper = conflictingTask.Paper,
                TaskType = StudyTask.Type.Conflict,
                DataFields = new List<DataField>(),
                IsEditable = true,
                Users = new List<User>()
            };

            foreach (var dataField in conflictingTask.DataFields)
            {
                task.DataFields.Add(new DataField
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