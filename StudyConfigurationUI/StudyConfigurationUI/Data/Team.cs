using System.Collections.Generic;

namespace StudyConfigurationUI.Data
{
    public class Team : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int[] UserIDs { get; set; } 
        public List<User> Users { get; set; } 
        public List<Study> Studies { get; set; } 
        public string Metadata { get; set; }
    }

    
}
