#region Using

using System.Collections.Generic;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement
{
    /// <summary>
    /// Class responsible for generating tasks
    /// </summary>
    public class TaskGenerator
    {
        /// <summary>
        /// Generate review tasks for every item
        /// </summary>
        /// <param name="item">item to generate a task for</param>
        /// <param name="criteria">lsit of criteria for a task to meet</param>
        /// <returns></returns>
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

        /// <summary>
        /// Generate validation task for every conflicting task
        /// </summary>
        /// <param name="conflictingTask">study task in conflict</param>
        /// <returns></returns>
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
                var newDataField = new DataField
                {
                    Description = dataField.Description,
                    FieldType = dataField.FieldType,
                    Name = dataField.Name,
                    UserData = new List<UserData>(),
                    ConflictingData = new List<UserData>()
                };
                
                foreach (var userData in dataField.UserData)
                {
                    newDataField.ConflictingData.Add(userData);
                }

                task.DataFields.Add(newDataField);

                var datacount = dataField.UserData.ToArray();
            }

            

            return task;
        }
    }
}