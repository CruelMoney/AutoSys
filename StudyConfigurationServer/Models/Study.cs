using System.Collections.Generic;
using System.Linq;
using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    public class Study : IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// The official Name of the study.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The DB id for the current stage
        /// </summary>
        public int CurrentStageID { get; set; }
        public List<Stage> Stages { get; set; } // reference til Stages (one to many)
        public Team Team { get; set; }
        public List<Item> Items { get; set; } // where to place?
        public bool IsFinished { get; set; }

        /// <summary>
        /// Finds the next stage db ID and returns it. 
        /// We rely on the database to keep the study's list of stages in order.
        /// </summary>
        /// <returns></returns>
        public int MoveToNextStage()
        {
            var currentIndex = Stages.FindIndex(s => s.Id.Equals(CurrentStageID));
            return CurrentStageID = Stages[currentIndex + 1].Id;
        }
    }
}