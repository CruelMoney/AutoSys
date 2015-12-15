#region Using

using System.ComponentModel.DataAnnotations;
using Storage.Repository;

#endregion

namespace StudyConfigurationServer.Models
{
    public class UserStudies : IEntity
    {
        /// <summary>
        ///     The users role in the given study.
        /// </summary>
        public enum Role
        {
            /// <summary>
            ///     The users role is to validate conflicting tasks.
            /// </summary>
            Validator,

            /// <summary>
            ///     The users role is to review tasks.
            /// </summary>
            Reviewer
        }

        [Required]
        public virtual User User { get; set; }

        [Required]
        public Stage Stage { get; set; }

        [Required]
        public Role StudyRole { get; set; }

        public int ID { get; set; }
    }
}