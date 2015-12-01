using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.CriteriaValidator;
using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model
{
    public class StageLogic : IEntity
    {
        public string Name { get; set;}
        public int Id { get; set; }
        public virtual List<Criteria> Criteria { get; set; } // reference til Criteria (one to many)
        public virtual List<TaskLogic> Tasks { get; set; } // reference til Task (one to many)
        public virtual StudyLogic Study { get; set; } // reference til Study (many to one)
    }
}
