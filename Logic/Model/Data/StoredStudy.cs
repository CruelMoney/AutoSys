using Logic.Model.DTO;
using Storage.Repository;
using System.Collections.Generic;

namespace Logic.Model.Data
{
    public class StoredStudy : StudyLogic,  IEntity
    {
        public int Id { get; set; }
    }
}
