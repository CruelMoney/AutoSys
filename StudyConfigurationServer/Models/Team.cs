#region Using

using System.Collections.Generic;
using Storage.Repository;

#endregion

namespace StudyConfigurationServer.Models
{
    public class Team : IEntity
    {
        public string Name { get; set; }
        public virtual List<User> Users { get; set; }
        public List<Study> Studies { get; set; }
        public string Metadata { get; set; }
        public int ID { get; set; }
    }
}