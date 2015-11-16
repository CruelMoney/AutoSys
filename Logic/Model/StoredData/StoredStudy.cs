using System;
using Storage.Repository;

namespace Logic.Model.StoredData
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