using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
using Storage.Repository;

namespace StudyConfigurationServer.Models
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
            return Data.Select(d => !"".Equals(d)).Any();
        }
     }
}
