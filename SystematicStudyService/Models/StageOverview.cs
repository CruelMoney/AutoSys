using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SystematicStudyService.Models
{
    /// <summary>
    /// The overview of a stage within a <see cref="StudyOverview"/>.
    /// </summary>
    public class StageOverview
    {
        /// <summary>
        /// An optional short name describing the key point of interest of this stage.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Per user ID, the amount of completed tasks in this stage.
        /// </summary>
        [Required]
        public Dictionary<int,int> CompletedTasks { get; set; }

        /// <summary>
        /// Per user ID, the amount of incomplete tasks in this stage.
        /// </summary>
        [Required]
        public Dictionary<int, int> IncompleteTasks { get; set; }
    }
}