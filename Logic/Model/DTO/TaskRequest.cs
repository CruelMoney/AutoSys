using System.ComponentModel.DataAnnotations;

namespace Logic.Model.DTO
{
    /// <summary>
    /// A requested task, part of a systematic study.
    /// </summary>
    public class TaskRequest
    {
        /// <summary>
        /// Defines whether the requested tasks are reviewing tasks, conflict tasks, or any task.
        /// </summary>
        public enum Type
        {
            Both,
            Review,
            Conflict
        }

        /// <summary>
        /// Filters task requests.
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
        /// A unique identifier for the task.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The <see cref="Type" /> of the task, either a review task, or a conflict task.
        /// </summary>
        [Required]
        public Type TaskType { get; set; }

        /// <summary>
        /// Defines whether the task is still deliverable or not.
        /// </summary>
        [Required]
        public bool IsDeliverable { get; set; }

        /// <summary>
        /// A list of data fields which are to be shown to the user, but are not editable.
        /// </summary>
        [Required]
        public DataField[] VisibleFields { get; set; }

        /// <summary>
        /// A list of requested data fields which need to be filled out as part of the task.
        /// </summary>
        [Required]
        public DataField[] RequestedFields { get; set; }

        /// <summary>
        /// In case this is a <see cref="Type.Conflict" /> task, represents for each of the <see cref="RequestedFields" /> the list of <see cref="ConflictingData" /> provided by separate users.
        /// </summary>
        public ConflictingData[][] ConflictingData { get; set; }
    }
}