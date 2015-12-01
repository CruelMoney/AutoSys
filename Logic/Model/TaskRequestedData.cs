using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model
{
    /// <summary>
    /// This class represents the data entered by a user for a specific task. 
    /// </summary>
    public  class TaskRequestedData
    {
        /// <summary>
        /// The user that is associated with this task and it's data
        /// </summary>
        public virtual UserLogic User { get; set; }
        /// <summary>
        /// The associated task
        /// </summary>
        public virtual TaskLogic Task { get; set; }
        /// <summary>
        /// The Data entered
        /// </summary>
        public string[] Data { get; set; } 
    }
}
