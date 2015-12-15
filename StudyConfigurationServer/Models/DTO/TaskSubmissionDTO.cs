#region Using

using System.ComponentModel.DataAnnotations;

#endregion

namespace StudyConfigurationServer.Models.DTO
{
    /// <summary>
    ///     A submission by a User for a requested StudyTask.
    /// </summary>
    public class TaskSubmissionDto
    {
        /// <summary>
        ///     The User who is submitting this StudyTask.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        ///     A list of the filled out data fields for this StudyTask. Only <see cref="DataFieldDto.Data" /> should be modified.
        /// </summary>
        [Required]
        public DataFieldDto[] SubmittedFieldsDto { get; set; }
    }
}