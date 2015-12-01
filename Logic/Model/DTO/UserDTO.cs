using System.ComponentModel.DataAnnotations;

namespace Logic.Model.DTO
{
    /// <summary>
    /// A user of the systematic study service.
    /// </summary>
    public class UserDTO
    {
        
        /// <summary>
        /// A unique identifier for the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name for the user.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Metadata can be used to store additional data related to the user, specific to a particular consumer of the API.
        /// </summary>
        public string Metadata { get; set; }
    }
}