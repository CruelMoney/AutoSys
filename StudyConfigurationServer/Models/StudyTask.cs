#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServer.Models
{
    public class StudyTask : IEntity
    {
        /// <summary>
        ///     Defines whether the requested tasks are reviewing tasks, conflict tasks, or any StudyTask.
        /// </summary>
        public enum Type
        {
            Both,
            Review,
            Conflict
        }

        /// <summary>
        ///     The StudyTask is connected to a certain paper
        /// </summary>
        public virtual Item Paper { get; set; }

        public List<User> Users { get; set; }

        /// <summary>
        ///     Defines wether the task can still be edited. Changes to false when all tasks for a stage has been delivered.
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        ///     The <see cref="Type" /> of the StudyTask, either a review StudyTask, or a conflict StudyTask.
        /// </summary>
        public Type TaskType { get; set; }

        /// <summary>
        ///     A the data which need to be filled out as part of the StudyTask.
        /// </summary>
        public virtual List<DataField> DataFields { get; set; }

        public Stage Stage { get; set; }

        /// <summary>
        ///     A unique identifier for the StudyTask.
        /// </summary>
        public int ID { get; set; }

        public StudyTask SubmitData(TaskSubmissionDto taskToDeliver)
        {
            var userId = taskToDeliver.UserId;

            var newDataFields = taskToDeliver.SubmittedFieldsDto.ToList();

            //TODO for now we use the dataField name to update the data.
            foreach (var field in newDataFields)
            {
                DataField fieldToUpdate;

                try
                {
                    fieldToUpdate = DataFields.First(d => d.Name.Equals(field.Name));
                }
                catch (Exception)
                {
                    throw new ArgumentException("A Corresponding dataField is not found in the task");
                }

                fieldToUpdate.SubmitData(userId, field.Data);
            }

            return this;
        }

        public bool IsFinished(int? userId = null)
        {
            if (userId == null)
            {
                return DataFields.TrueForAll(d => d.DataEntered());
            }
            try
            {
                return DataFields.TrueForAll(d => d.DataEntered(userId));
            }
            catch (Exception)
            {
                throw new ArgumentException("The user is not associated with this task");
            }
        }

        public bool ContainsConflictingData()
        {
            return DataFields.Any(d => d.UserDataIsConflicting());
        }
    }
}