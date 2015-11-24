﻿using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class StoredStageOverview : StageOverview, IEntity
    {
        public int Id { get; set; }
    }
}
