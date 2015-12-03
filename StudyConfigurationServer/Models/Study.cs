using System.Collections.Generic;
using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    public class Study : IEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int CurrentStage { get; set; }
        public bool IsFinished { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }  // reference til TeamDTO (many to one)
        public virtual List<Stage> Stages { get; set; } // reference til Stages (one to many)
        public virtual List<Item> Items { get; set; } // where to place?
    }
}