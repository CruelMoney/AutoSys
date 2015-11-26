﻿using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class StoredConflictingData : ConflictingData, IEntity
    {
        public int Id { get; set; }
    }
}