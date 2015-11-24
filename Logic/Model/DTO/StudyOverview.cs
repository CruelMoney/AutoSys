using System.ComponentModel.DataAnnotations;

namespace Logic.Model.DTO
{
    /// <summary>
    /// The overview of a systematic study on which a team works.
    /// </summary>
    public class StudyOverview
    {
        /// <summary>
        /// A name for the study.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// A list of IDs of users who work on the study.
        /// </summary>
        [Required]
        public int[] UserIds { get; set; }

        /// <summary>
        /// An overview of the state of the different stages in the study.
        /// </summary>
        [Required]
        public PhaseOverview[] Stages { get; set; }
    }
}