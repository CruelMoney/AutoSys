using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model
{
    public class TeamLogic : IEntity
    {
        public int Id { get; set; }
        public string Name { get;  set}
        public int UserId { get; set; }
        public virtual List<UserLogic> Users { get; set; }
    }
}
