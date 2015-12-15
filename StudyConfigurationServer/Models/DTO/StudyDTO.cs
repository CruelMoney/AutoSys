#region Using

using System.Linq;

#endregion

namespace StudyConfigurationServer.Models.DTO
{
    public class StudyDto
    {
        public StudyDto(){}

        public StudyDto(Study study)
        {
            Id = study.ID;
            Name = study.Name;
            Stages = study.Stages.Select(s => new StageDto(s)).ToArray();
            Team = new TeamDto(study.Team);
            Items = new byte[] {};
            IsFinished = study.IsFinished;
        }

        public int Id { get; set; }

        /// <summary>
        ///     The official Name of the study.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The DB id for the current stage
        /// </summary>
        public StageDto[] Stages { get; set; } // reference til Stages (one to many)
        public TeamDto Team { get; set; }
        public byte[] Items { get; set; } // where to place?
        public bool IsFinished { get; set; }
    }
}