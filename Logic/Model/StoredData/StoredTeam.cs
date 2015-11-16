using System;
using Storage.Repository;

namespace Logic.Model.StoredData
{
    public class StoredTeam : IEntity
    {
        public int Id { get; set; }
        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
