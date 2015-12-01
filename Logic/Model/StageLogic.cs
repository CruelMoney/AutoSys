using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.Model
{
    public class StageLogic : IEntity
    {
        public string Name { get; set;}
        public int Id { get; set; }
        public int CriteriaId { get; set; }
        public virtual List<CriteriaLogic> Criteria { get; set; }
        public int TaskId { get; set; }
        public virtual List<TaskLogic> Tasks { get; set; }

    }
}
