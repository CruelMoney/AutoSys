using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model
{
    public class Team : IEntity
    {
        public int Id { get; set; }
        public string Name { get;  set}
        public int UserId { get; set; }
        public virtual List<User> Users { get; set; }
    }
}
