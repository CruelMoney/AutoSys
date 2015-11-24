﻿using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class ResourceField : Resource, IEntity
    {
        public int Id { get; set; }
    }
}
