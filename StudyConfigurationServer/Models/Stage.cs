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
        public  List<Criteria> Criteria { get; set; } 
        public List<StudyTask> Tasks { get; set; }
        public Study Study { get; set; } 
        //The fields that can only be seen in adddition to the editable fields.
        public List<Item.FieldType> VisibleFields { get; set; }
        public List<UserStudies> Users { get; set; }
        /// <summary>
        /// Defines wether the stage is for reviewing or validating tasks.
        /// </summary>
        public StudyTask.Type StageType { get; set; }
        public Distribution DistributionRule { get; set; }

        public enum Distribution
        {
            FiftyPercentOverlap,
            HundredPercentOverlap,
            NoOverlap,
        }

        public bool IsFinished()
        {
            return Tasks.TrueForAll(t => t.IsFinished());
        }
    }
}
