using System;
using System.Collections.Generic;

namespace Logic.Model
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
