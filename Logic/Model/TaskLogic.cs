using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.DTO;

namespace Logic.Model
{
    public class TaskLogic : IEntity
    {

        /// <summary>
        /// Defines whether the requested tasks are reviewing tasks, conflict tasks, or any task.
        /// </summary>
        public enum Type
        {
            Both,
            Review,
            Conflict
        }

        /// <summary>
        /// Filters task requests.
        /// </summary>
        public enum Filter
        {
            /// <summary>
            /// Only list remaining tasks.
            /// </summary>
            Remaining,
            /// <summary>
            /// Only list delivered tasks which are still editable.
            /// </summary>
            Editable,
            /// <summary>
            /// Only list tasks which are done, and are no longer editable.
            /// </summary>
            Done
        }
        /// <summary>
        /// The task is connected to a certain paper
        /// </summary>
        public virtual ItemLogic Paper { get; set; }

        public virtual StageLogic Stage { get; set; } // reference to Stage (many to one)

        /// <summary>
        /// A unique identifier for the task.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The <see cref="Type" /> of the task, either a review task, or a conflict task.
        /// </summary>
        public Type TaskType { get; set; }

        /// <summary>
        /// Defines whether the task is still deliverable or not.
        /// </summary>
        public bool IsDeliverable { get; set; }

        /// <summary>
        /// A list of data fields which are to be shown to the user, but are not editable.
        /// </summary>
        public DataFieldLogic[] VisibleFieldsLogic { get; set; }

        /// <summary>
        /// A the data which need to be filled out as part of the task.
        /// </summary>
        public virtual TaskRequestedData RequestedData { get; set; }
    }
}
