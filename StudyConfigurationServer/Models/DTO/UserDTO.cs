using System.ComponentModel.DataAnnotations;

namespace StudyConfigurationServer.Models.DTO
{
    /// <summary>
    /// A User of the systematic study service.
    /// </summary>
    public class UserDTO
    {   
        /// <summary>
        /// A unique identifier for the User.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name for the User.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Metadata can be used to store additional data related to the User, specific to a particular consumer of the API.
        /// </summary>
        public string Metadata { get; set; }

        public UserDTO(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Metadata = user.Metadata;
        }

        public UserDTO()
        {
            
        }
    }
    
}