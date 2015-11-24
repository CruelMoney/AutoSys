using System.ComponentModel.DataAnnotations;
using SystematicStudyService.Controllers;

namespace SystematicStudyService.Models
{
    /// <summary>
    /// Describes a resource data field.
    /// </summary>
    public class Resource
    {
        /// <summary>
        /// The unique ID for the resource, which can be used to retrieve the resource by calling <see cref="StudyController.GetResource" />.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// The file extension of the resource.
        /// </summary>
        [Required]
        public string Extension { get; set; }

        /// <summary>
        /// The file size of the resource.
        /// </summary>
        [Required]
        public int Size { get; set; }
    }
}