using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
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
        public int UserID { get; set; }
      
        /// <summary>
        /// The Data entered
        /// </summary>
        public virtual string[] Data { get; set; } 

        public int Id { get; set; }

        public bool ContainsData()
        {
            if (Data==null)
            {
                return false;
            }
            return Data.Where(d => !d.IsNullOrWhiteSpace()).Any();
        }

        public bool DataEquals(UserData userData)
        {
            return Data.Equals(userData.Data);
        }
    }
}
