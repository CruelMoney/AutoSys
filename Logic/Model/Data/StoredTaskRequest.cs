﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class StoredTaskRequest : TaskRequest, IEntity
    {
        public int Id { get; set; }
    }
}
