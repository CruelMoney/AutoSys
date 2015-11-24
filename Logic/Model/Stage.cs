using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.DTO;

namespace Logic.Model
{
    public class Stage
    {
        public Criteria Criteria { get; set; }
        public List<User> Users { get; set; }
        public TaskRequest.Type StageType { get; set; }
    }
}
