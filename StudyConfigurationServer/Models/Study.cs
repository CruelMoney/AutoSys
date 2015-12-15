#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using Storage.Repository;

#endregion

namespace StudyConfigurationServer.Models
{
    public class Study : IEntity
    {
        /// <summary>
        ///     The official Name of the study.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The DB id for the current stage
        /// </summary>
        public virtual ICollection<Stage> Stages { get; set; } // reference til Stages (one to many)

        public virtual Team Team { get; set; }
        public virtual List<Item> Items { get; set; } // where to place?
        public bool IsFinished { get; set; }

        public int ID { get; set; }


        /// <summary>
        ///     Changes the currentstage id to the next and returns it.
        ///     We rely on the database to keep the study's list of stages in order.
        /// </summary>
        /// <returns></returns>
        public void MoveToNextStage()
        {
            if (!Items.Any())
            {
                IsFinished = true;
            }
            else
            {
                var currentStage = Stages.ToList().First(s => s.IsCurrentStage);
                var currentIndex = Stages.ToList().FindIndex(s => s.IsCurrentStage);
                currentStage.IsCurrentStage = false;
                if (currentIndex + 1 == Stages.Count)
                {
                    IsFinished = true;
                }
                else
                {
                    Stages.ToList()[currentIndex + 1].IsCurrentStage = true;
                }
            }
        }

        public Stage CurrentStage()
        {
            try
            {
                return Stages.First(s => s.IsCurrentStage);
            }
            catch (Exception)
            {
                throw new NullReferenceException("No stage set to current stage");
            }
        }
    }
}