﻿using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class StoredDataField : DataField, IEntity
    {
        public int Id { get; set; }
    }
}