using System.Collections.Generic;

namespace ConsoleApplication1.Data
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
