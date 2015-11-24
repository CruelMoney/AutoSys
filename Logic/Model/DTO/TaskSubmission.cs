using System.ComponentModel.DataAnnotations;

namespace Logic.Model.DTO
{
    /// <summary>
    /// A submission by a user for a requested task.
    /// </summary>
    public class TaskSubmission
    {
        /// <summary>
        /// The user who is submitting this task.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// A list of the filled out data fields for this task. Only <see cref="DataField.Data" /> should be modified.
        /// </summary>
        [Required]
        public DataField[] SubmittedFields { get; set; }
    }
}