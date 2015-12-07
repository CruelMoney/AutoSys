using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Models
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<Team> Teams { get; set; }
        public virtual List<UserStudies> Studies { get; set; }
        public virtual List<Stage> Stages { get; set; }
        public string Metadata { get; set; }
        public virtual List<TaskRequestedData> Tasks { get; set; }
       
    }
}
