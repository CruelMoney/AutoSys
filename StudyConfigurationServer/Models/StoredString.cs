﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    public class StoredString : IEntity
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }
}