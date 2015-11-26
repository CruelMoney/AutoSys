using System.Collections.Generic;
using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model
{
    public class StudyLogic : IEntity
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int CurrentStage { get; set; }
        public bool IsFinished { get; set; }
        public int TeamId { get; set; }
        public virtual TeamLogic Team { get; set; }  // reference til Team (many to one)
        public virtual List<StageLogic> Stages { get; set; } // reference til Stages (one to many)
        public virtual List<ItemLogic> Items { get; set; } // where to place?
    }
}