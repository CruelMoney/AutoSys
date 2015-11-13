using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Data
{
    class Phase
    {
        public List<Task> TasksToDo { get; set; }
        public List<Task> Finishedtask { get; set; }
        public Boolean isFinished { get; set; }
        public enum PhaseType
        {
            ReviewPhase,
            ValidatePhase
         }


     }
}
