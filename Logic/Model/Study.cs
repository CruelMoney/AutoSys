using System.Collections.Generic;
using Logic.Model.DTO;

namespace Logic.Model
{
    public class Study
    {
        public List<Stage> Stages { get; set; } 
        public List<Item> Items { get; set; } 
        public int CurrentStage { get; set; }
        public bool IsFinished { get; set; }
        public Team Team { get; set; }
    }
}