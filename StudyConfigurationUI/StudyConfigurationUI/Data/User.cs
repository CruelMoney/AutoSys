using System.Collections.Generic;

namespace StudyConfigurationUI.Data
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
        public List<UserStudies> Stages { get; set; }
        public string Metadata { get; set; }
        public List<UserData> Tasks { get; set; }
       
    }
}
