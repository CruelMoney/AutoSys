using System.Collections.Generic;
using System.Linq;

namespace StudyConfigurationUI.Data
{
    /// <summary>
    /// This class represents the data entered by a User for a specific StudyTask. 
    /// </summary>
    public  class TaskRequestedData : IEntity
    {
        /// <summary>
        /// The User that is associated with this StudyTask and it's data
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// The associated StudyTask
        /// </summary>
        public StudyTask StudyTask { get; set; }
        /// <summary>
        /// The Data entered
        /// </summary>
        public List<DataField> Data { get; set; } 

        public int Id { get; set; }

        /// <summary>
        /// Defines whether the StudyTask for this user is finished.
        /// </summary>
        public bool IsFinished { get; set; }

        /// <summary>
        /// Defines whether the StudyTask is still deliverable for this user or not.
        /// </summary>
        public bool IsDeliverable { get; set; }

        public bool IsTaskFinished()
        {
            return Data.All(field => field.Data.Any());
        }
    }
}
