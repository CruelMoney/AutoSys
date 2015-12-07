using System.Collections.Generic;
using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    /// <summary>
    /// This class represents the data entered by a User for a specific StudyTask. 
    /// </summary>
    public  class TaskRequestedData : IEntity
    {
        /// <summary>
        /// The User that is associated with this StudyTask and it's data
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// The associated StudyTask
        /// </summary>
        public virtual StudyTask StudyTask { get; set; }
        /// <summary>
        /// The Data entered
        /// </summary>
        public List<DataField> Data { get; set; } 

        public int Id { get; set; }
    }
}
