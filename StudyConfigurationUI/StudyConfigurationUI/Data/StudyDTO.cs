namespace StudyConfigurationUI.Data
{
    public class StudyDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// The official Name of the study.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The DB id for the current stage
        /// </summary>
        public StageDTO[] Stages { get; set; } // reference til Stages (one to many)
        public TeamDTO Team { get; set; }
        public byte[] Items { get; set; } // where to place?
        public bool IsFinished { get; set; }
    }
}