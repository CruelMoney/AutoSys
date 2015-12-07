using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Storage.Repository;

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
        /// Filters StudyTask requests.
        /// </summary>
        public enum Filter
        {
            /// <summary>
            /// Only list remaining tasks.
            /// </summary>
            Remaining,
            /// <summary>
            /// Only list delivered tasks which are still editable.
            /// </summary>
            Editable,
            /// <summary>
            /// Only list tasks which are done, and are no longer editable.
            /// </summary>
            Done
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

        public virtual Stage Stage { get; set; } // reference to Stage (many to one)

        

        /// <summary>
        /// The <see cref="Type" /> of the StudyTask, either a review StudyTask, or a conflict StudyTask.
        /// </summary>
        public Type TaskType { get; set; }

        /// <summary>
        /// Defines whether the StudyTask is finished.
        /// </summary>
        public bool IsFinished { get; set; }

        /// <summary>
        /// Defines whether the StudyTask is still deliverable or not.
        /// </summary>
        public bool IsDeliverable { get; set; }

        /// <summary>
        /// A the data which need to be filled out as part of the StudyTask.
        /// </summary>
        public virtual List<TaskRequestedData> RequestedData { get; set; }
    }
}
