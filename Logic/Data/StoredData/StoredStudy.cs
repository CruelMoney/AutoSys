using System.Data.Entity;
using Storage.Repository;
using System;
using System.Collections.Generic;

namespace Logic.Data
{
    public class StoredStudy : IEntity
    {
        public int Id { get; set; }
    }

}