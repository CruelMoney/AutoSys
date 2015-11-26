using System.Collections.Generic;
using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model
{
    public class Study : IEntity
    {
        public int Id { get; set; }
        public int CurrentStage { get; set; }
        public bool IsFinished { get; set; }
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }
        public int StageId { get; set; }
        public virtual List<Stage> Stages { get; set; }
        public int ItemId { get; set; }
        public virtual List<Item> Items { get; set; } // where to place?
    }
}