using System.ComponentModel.DataAnnotations;

namespace Logic.Model.DTO
{
    /// <summary>
    /// A submission by a user for a requested StudyTask.
    /// </summary>
    public class TaskSubmissionDTO
    {
        /// <summary>
        /// The user who is submitting this StudyTask.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// A list of the filled out data fields for this StudyTask. Only <see cref="DataFieldDTO.Data" /> should be modified.
        /// </summary>
        [Required]
        public DataFieldDTO[] SubmittedFieldsDto { get; set; }
    }
}