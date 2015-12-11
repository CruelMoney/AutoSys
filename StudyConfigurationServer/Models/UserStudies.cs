using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    public class UserStudies : IEntity
    {
        public virtual User User { get; set; }
        public Stage Stage { get; set; }
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