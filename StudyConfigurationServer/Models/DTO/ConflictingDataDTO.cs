﻿#region Using

using System.ComponentModel.DataAnnotations;

#endregion

namespace StudyConfigurationServer.Models.DTO
{
    /// <summary>
    ///     Represents data of one User provided for a <see cref="DataFieldDto" />, used to indicate conflicting data between
    ///     users.
    /// </summary>
    public class ConflictingDataDto
    {
        /// <summary>
        ///     The User ID of the User who provided the data.
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        ///     The data provided by the User.
        /// </summary>
        [Required]
        public string[] Data { get; set; }
    }
}