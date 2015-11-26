using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
