using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Models
{
    public class Study : IEntity
    {
        
        public int ID { get; set; }
        /// <summary>
        /// The official Name of the study.
        /// </summary>
        public string Name { get; set; }
        public int CurrentStageID { get; set; } 
        /// <summary>
        /// The DB id for the current stage
        /// </summary>
        public virtual ICollection<Stage> Stages { get; set; } // reference til Stages (one to many)


        public virtual Team Team { get; set; }
        public virtual List<Item> Items { get; set; } // where to place?
        public bool IsFinished { get; set; }


        /// <summary>
        /// Changes the currentstage id to the next and returns it. 
        /// We rely on the database to keep the study's list of stages in order.
        /// </summary>
        /// <returns></returns>
        public void MoveToNextStage()
        {
            var currentStage = Stages.ToList().First(s => s.IsCurrentStage);
            currentStage.IsCurrentStage = false;
            var currentIndex = Stages.ToList().FindIndex(s=>s.IsCurrentStage);
            Stages.ToList()[currentIndex + 1].IsCurrentStage = true;
        }

        public Stage CurrentStage()
        {
            return Stages.First(s => s.IsCurrentStage);
        }
        
    }
}