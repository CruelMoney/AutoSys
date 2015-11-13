﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.Repository;

namespace Logic.Data
{
    public class Team : IEntity
    {
        public Team(String TeamName, IEnumerable<User> UserList, String MetaData)
        {
            Name = TeamName;
            TeamList.AddRange(UserList);
            this.MetaData = MetaData;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string MetaData { get; set; }
        public List<User> TeamList { get; set; }

        
    }
}
