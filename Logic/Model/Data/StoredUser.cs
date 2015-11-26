using System;
using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class StoredUser : IEntity
    {
        public int Id { get; set; }
    }
}
