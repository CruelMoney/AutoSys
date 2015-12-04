using System.ComponentModel.DataAnnotations;

namespace StudyConfigurationServer.Models.DTO
{
    /// <summary>
    /// A submission by a User for a requested StudyTask.
    /// </summary>
    public class TaskSubmissionDTO
    {
        /// <summary>
        /// The User who is submitting this StudyTask.
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