#region Using

using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Storage.Repository;

#endregion

namespace StudyConfigurationServer.Models
{
    /// <summary>
    ///     This class represents the data entered by a User for a specific StudyTask.
    /// </summary>
    public class UserData : IEntity
    {
        /// <summary>
        ///     The User that is associated with this StudyTask and it's data
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     The Data entered
        /// </summary>
        public virtual List<StoredString> Data { get; set; }

        public int ID { get; set; }

        public bool ContainsData()
        {
            return Data != null && Data.Any(d => !d.Value.IsNullOrWhiteSpace());
        }

        public bool DataEquals(UserData userData)
        {
            return Data.SequenceEqual(userData.Data);
        }
    }
}