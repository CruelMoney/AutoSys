using System.Linq;


namespace StudyConfigurationUI.Data
{
    /// <summary>
    /// This class represents the data entered by a User for a specific StudyTask. 
    /// </summary>
    public  class UserData : IEntity
    {
        /// <summary>
        /// The User that is associated with this StudyTask and it's data
        /// </summary>
        public User User { get; set; }
      
        /// <summary>
        /// The Data entered
        /// </summary>
        public string[] Data { get; set; } 

        public int Id { get; set; }

        public bool ContainsData()
        {
            if (Data==null)
            {
                return false;
            }
            return true; // Data.Where(d => !d.IsNullOrWhiteSpace()).Any();
        }
     }
}
