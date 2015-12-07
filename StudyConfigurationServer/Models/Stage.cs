using System.Collections.Generic;
using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    public class Stage : IEntity
    {
        public string Name { get; set;}
        public int Id { get; set; }
        //The criteria are defining what fields are editable for this stage
        public virtual List<Criteria> Criteria { get; set; } 
        public virtual List<StudyTask> Tasks { get; set; }
        public virtual Study Study { get; set; } 
        //The fields that can only be seen in adddition to the editable fields.
        public List<Item.FieldType> VisibleFields { get; set; }
        public virtual List<UserStudies> Users { get; set; }
    }
}
