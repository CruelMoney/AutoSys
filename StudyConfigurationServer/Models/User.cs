#region Using

using System.Collections.Generic;
using Storage.Repository;

#endregion

namespace StudyConfigurationServer.Models
{
    public class User : IEntity
    {
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
        public List<UserStudies> Stages { get; set; }
        public List<StudyTask> Tasks { get; set; }
        public string Metadata { get; set; }
        public int ID { get; set; }
    }
}