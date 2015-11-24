﻿using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class StoredTaskSubmission : TaskSubmission, IEntity
    {
        public int Id { get; set; }
    }
}
