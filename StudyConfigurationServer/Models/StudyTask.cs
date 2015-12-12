using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Models
{
    public class StudyTask : IEntity
    {

        /// <summary>
        /// Defines whether the requested tasks are reviewing tasks, conflict tasks, or any StudyTask.
        /// </summary>
        public enum Type
        {
            Both,
            Review,
            Conflict
        }

        /// <summary>
        /// A unique identifier for the StudyTask.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// The StudyTask is connected to a certain paper
        /// </summary>
        [Required]
        public virtual Item Paper { get; set; }
        public List<int> UserIDs { get; set; } 
        /// <summary>
        /// Defines wether the task can still be edited. Changes to false when all tasks for a stage has been delivered. 
        /// </summary>
        public bool IsEditable { get; set; }

        /// <summary>
        /// The <see cref="Type" /> of the StudyTask, either a review StudyTask, or a conflict StudyTask.
        /// </summary>
        public Type TaskType { get; set; }

        /// <summary>
        /// A the data which need to be filled out as part of the StudyTask.
        /// </summary>
        public virtual List<DataField> DataFields { get; set; }

        public StudyTask SubmitData(TaskSubmissionDTO taskToDeliver)
        {
            var userID = taskToDeliver.UserId;
            
            var newDataFields = taskToDeliver.SubmittedFieldsDto.ToList();
            
            //TODO for now we use the dataField name to update the data.
            foreach (var field in newDataFields)
            {
                var fieldToUpdate = DataFields.First(d=>d.Name.Equals(field.Name));
                
                if (fieldToUpdate==null)
                {
                    throw new InvalidOperationException("A Corresponding dataField is not found in the task");
                }

                fieldToUpdate.SubmitData(userID, field.Data);

            }

            return this;
        }

        public bool IsFinished(int? userID = null)
        {
            if (userID == null)
            {
                return DataFields.TrueForAll(d => d.DataEntered());
            }
            try
            {
                return DataFields.TrueForAll(d => d.DataEntered(userID));
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
