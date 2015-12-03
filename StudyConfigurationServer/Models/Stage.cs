using System.Collections.Generic;
using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    public class Stage : IEntity
    {
        public string Name { get; set;}
        public int Id { get; set; }
        public virtual List<Criteria> Criteria { get; set; } // reference til Criteria (one to many)
        public virtual List<StudyTask> Tasks { get; set; } // reference til StudyTask (one to many)
        public virtual Study Study { get; set; } // reference til Study (many to one)
    }
}
