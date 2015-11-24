using System;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class StoredStudy : IEntity
    {
        public int Id { get; set; }
        public void Update(Study study)
        {
            throw new NotImplementedException();
        }
    }

}