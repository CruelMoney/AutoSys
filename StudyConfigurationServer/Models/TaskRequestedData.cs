using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    /// <summary>
    /// This class represents the data entered by a user for a specific StudyTask. 
    /// </summary>
    public  class TaskRequestedData : IEntity
    {
        /// <summary>
        /// The user that is associated with this StudyTask and it's data
        /// </summary>
        public virtual User User { get; set; }
        /// <summary>
        /// The associated StudyTask
        /// </summary>
        public virtual StudyTask StudyTask { get; set; }
        /// <summary>
        /// The Data entered
        /// </summary>
        public string[] Data { get; set; } 

        public int Id { get; set; }
    }
}
