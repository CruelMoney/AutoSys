using System.ComponentModel.DataAnnotations;

namespace SystematicStudyService.Models
{
    /// <summary>
    /// Represents data of one user provided for a <see cref="DataField" />, used to indicate conflicting data between users.
    /// </summary>
    public class ConflictingData
    {
        /// <summary>
        /// The user ID of the user who provided the data.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// The data provided by the user.
        /// </summary>
        [Required]
        public string[] Data { get; set; }
    }
}