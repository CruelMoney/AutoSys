using System.Collections.Generic;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models.DTO;

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
        public virtual List<Stage> Stages { get; set; } // reference til Stages (one to many)
        public virtual Team Team { get; set; }
        public virtual List<Item> Items { get; set; } // where to place?
        public bool IsFinished { get; set; }


        /// <summary>
        /// Changes the currentstage id to the next and returns it. 
        /// We rely on the database to keep the study's list of stages in order.
        /// </summary>
        /// <returns></returns>
        public int MoveToNextStage()
        {
            if (CurrentStageID == 0)
            {
                return CurrentStageID = Stages[0].Id;
            }
            var currentIndex = Stages.FindIndex(s => s.Id.Equals(CurrentStageID));
            return CurrentStageID = Stages[currentIndex + 1].Id;
        }

        public Stage CurrentStage()
        {
            return Stages.First(s => s.Id == CurrentStageID);
        }
    }
}