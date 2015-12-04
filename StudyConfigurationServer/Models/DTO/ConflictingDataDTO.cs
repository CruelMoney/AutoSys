using System.ComponentModel.DataAnnotations;

namespace StudyConfigurationServer.Models.DTO
{
    /// <summary>
    /// Represents data of one user provided for a <see cref="DataFieldDTO" />, used to indicate conflicting data between users.
    /// </summary>
    public class ConflictingDataDTO
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