using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StudyConfigurationServer.Models.DTO
{
    /// <summary>
    /// A team using the systematic study service.
    /// </summary>
    public class TeamDTO
    {
        
        /// <summary>
        /// A unique identifier for the team.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name for the team.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// The IDs of the users part of the team.
        /// </summary>
        [Required]
        public int[] UserIDs { get; set; }

        /// <summary>
        /// Metadata can be used to store additional data related to the team, specific to a particular consumer of the API.
        /// </summary>
        public string Metadata { get; set; }

        public TeamDTO(Team team)
        {
            Id = team.ID;
            Name = team.Name;
            UserIDs = team.Users.Select(u => u.ID).ToArray();
            Metadata = team.Metadata;
        }
        public TeamDTO()
        {
        }
    }
}