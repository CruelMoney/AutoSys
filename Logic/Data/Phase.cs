using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Data
{
    public class Phase
    {


        public Criteria PhaseCriteria;
        public Dictionary<User, List<UserTask>> userTasks { get; set; }
        public List<UserTask> finishedtask { get; set; }
        public List<UserTask> conflictingTask { get; set; }
        public Boolean isFinished { get; set; }
        public enum PhaseType
        {
            ReviewPhase,
            ValidatePhase
        }

        
       


     }
}
