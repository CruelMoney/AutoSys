using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.Repository;

namespace Logic.Data
{
    class Task : IEntity
    {
        public int Id { get; set; }
        public User AssociatedUser { get; set; }
        public Boolean TaskDone { get; set; }

    }
}
