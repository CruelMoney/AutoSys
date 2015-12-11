using System.ComponentModel.DataAnnotations;
using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    public class UserStudies : IEntity
    {
        [Required]
        public virtual User User { get; set; }
        [Required]
        public Stage Stage { get; set; }
        [Required]
        public Role StudyRole { get; set; }
        public int Id { get; set; }

        /// <summary>
        /// The users role in the given study.
        /// </summary>
        public enum Role
        {
            /// <summary>
            /// The users role is to validate conflicting tasks.
            /// </summary>
            Validator,
            /// <summary>
            /// The users role is to review tasks.
            /// </summary>
            Reviewer
         }
    }
}