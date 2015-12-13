using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
        public List<UserStudies> Stages { get; set; }
        public List<int> StudyIds { get; set; }
        public List<StudyTask> Tasks { get; set; } 
        public string Metadata { get; set; }
       
       
    }
}
