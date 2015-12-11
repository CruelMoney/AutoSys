using System.Collections.Generic;
using System.Linq;
using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    public class Stage : IEntity
    {
        public string Name { get; set;}
        public int Id { get; set; }
        //The criteria are defining what fields are editable for this stage
        public virtual ICollection<Criteria> Criteria { get; set; } 
        public ICollection<int> TaskIDs { get; set; }
        public int StudyID { get; set; } 
        //The fields that can only be seen in adddition to the editable fields.
        public ICollection<Item.FieldType> VisibleFields { get; set; }
        public virtual ICollection<UserStudies> Users { get; set; }
        /// <summary>
        /// Defines wether the stage is currently reviewing or validating.
        /// </summary>
        public StudyTask.Type CurrentTaskType { get; set; }
        public Distribution DistributionRule { get; set; }

        public enum Distribution
        {
            FiftyPercentOverlap,
            HundredPercentOverlap,
            NoOverlap,
        }
    }
}
