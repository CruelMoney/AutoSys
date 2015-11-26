using Logic.Model.DTO;
using Storage.Repository;
using System.Collections.Generic;

namespace Logic.Model.Data
{
    public class StoredStudy : StudyLogic,  IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Item> studyData { get; set; }
    }
}
