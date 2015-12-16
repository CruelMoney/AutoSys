namespace StudyConfigurationUILibrary
{
    /// <summary>
    ///     Passed parameter when navigating to <see cref="ManageStudyPage" />.
    /// </summary>
    public class ManageStudyPageArgs
    {
        private ManageStudyPageArgs()
        {
        }

        /// <summary>
        ///     The ID of the study to load.
        ///     When this is set, <see cref="TeamId" /> should be null.
        /// </summary>
        public int? StudyId { get; private set; }

        /// <summary>
        ///     The ID of the team for which to create a new study.
        ///     When this is set, <see cref="StudyId" /> should be null.
        /// </summary>
        public int? TeamId { get; private set; }

        /// <summary>
        ///     Create a parameter to be passed to <see cref="ManageStudyPage" /> for it to load an existing study.
        /// </summary>
        /// <param name="studyId">The study to load.</param>
        public static ManageStudyPageArgs CreateForExistingStudy(int studyId)
        {
            return new ManageStudyPageArgs
            {
                StudyId = studyId
            };
        }

        /// <summary>
        ///     Create a parameter to be passed to <see cref="ManageStudyPage" /> for it to create a new study for a given team.
        /// </summary>
        /// <param name="teamId">The team for which to create a new study.</param>
        public static ManageStudyPageArgs CreateForExistingTeam(int teamId)
        {
            return new ManageStudyPageArgs
            {
                TeamId = teamId
            };
        }
    }
}