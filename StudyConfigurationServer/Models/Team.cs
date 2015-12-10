using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Models
{
    public class Team : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] UserIDs { get; set; } 
        public virtual List<User> Users { get; set; } 
        public List<Study> Studies { get; set; } 
        public string Metadata { get; set; }
    }

    
}
