﻿using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class StoredStudyOverview : StudyOverview, IEntity
    {
        public int Id { get; set; }
    }
}