using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Team> Teams { get; set; }     // reference til Teams (one to many)
        public string Metadata { get; set; }
        public virtual List<StudyTask> Tasks { get; set; } 
    }
}
