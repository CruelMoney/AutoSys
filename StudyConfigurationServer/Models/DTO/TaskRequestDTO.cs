using System.ComponentModel.DataAnnotations;

namespace StudyConfigurationServer.Models.DTO
{
    /// <summary>
    /// A requested StudyTask, part of a systematic study.
    /// </summary>
    public class TaskRequestDTO
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
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The <see cref="Type" /> of the StudyTask, either a review StudyTask, or a conflict StudyTask.
        /// </summary>
        [Required]
        public Type TaskType { get; set; }

        /// <summary>
        /// Defines whether the StudyTask is still deliverable or not.
        /// </summary>
        [Required]
        public bool IsDeliverable { get; set; }

        /// <summary>
        /// A list of data fields which are to be shown to the User, but are not editable.
        /// </summary>
        [Required]
        public DataFieldDTO[] VisibleFieldsDto { get; set; }

        /// <summary>
        /// A list of requested data fields which need to be filled out as part of the StudyTask.
        /// </summary>
        [Required]
        public DataFieldDTO[] RequestedFieldsDto { get; set; }

        /// <summary>
        /// In case this is a <see cref="Type.Conflict" /> StudyTask, represents for each of the <see cref="RequestedFieldsDto" /> the list of <see cref="ConflictingDataDto" /> provided by separate users.
        /// </summary>
        public ConflictingDataDTO[][] ConflictingDataDto { get; set; }
    }
}