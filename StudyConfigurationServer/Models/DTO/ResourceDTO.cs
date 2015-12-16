#region Using

using System.ComponentModel.DataAnnotations;
using StudyConfigurationServer.Api;

#endregion

namespace StudyConfigurationServer.Models.DTO
{
    /// <summary>
    ///     Describes a resource data Field.
    /// </summary>
    public class ResourceDto
    {
        /// <summary>
        ///     The unique ID for the resource, which can be used to retrieve the resource by calling
        ///     <see cref="StudyController.GetResource" />.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        ///     The file extension of the resource.
        /// </summary>
        [Required]
        public string Extension { get; set; }

        /// <summary>
        ///     The file size of the resource.
        /// </summary>
        [Required]
        public int Size { get; set; }
    }
}