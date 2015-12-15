#region Using

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#endregion

namespace StudyConfigurationServer.Models.DTO
{
    public class StageDto
    {
        public enum Distribution
        {
            FiftyPercentOverlap,
            HundredPercentOverlap,
            NoOverlap
        }

        public enum FieldType
        {
            Address,
            Annote,
            Author,
            Booktitle,
            Chapter,
            Crossref,
            Edition,
            Editor,
            HowPublished,
            Instritution,
            Journal,
            Key,
            Month,
            Note,
            Number,
            Organization,
            Pages,
            Publisher,
            School,
            Series,
            Title,
            Type,
            Volume,
            Year,
            Url,
            Isbn,
            Issn,
            Lccn,
            Abstract,
            Keywords,
            Price,
            Copyright,
            Language,
            Contents,
            Doi
        }

        public StageDto(Stage stage)
        {
            Name = stage.Name;
            Id = stage.ID;
            Criteria = new CriteriaDto(stage.Criteria.ElementAt(0));
            ReviewerIDs = (from u in stage.Users where u.StudyRole == UserStudies.Role.Reviewer select u.ID).ToArray();
            ValidatorIDs = (from u in stage.Users where u.StudyRole == UserStudies.Role.Validator select u.ID).ToArray();
            DistributionRule = (Distribution) Enum.Parse(typeof (Stage.Distribution), stage.DistributionRule.ToString());
            VisibleFields =
                stage.VisibleFields.Select(
                    vf => (FieldType) Enum.Parse(typeof (Models.FieldType.TypEField), vf.Type.ToString())).ToArray();
        }

        public StageDto(){}

        public string Name { get; set; }
        public int Id { get; set; }
        //The critderia are defining what fields are going to be editable for this stage
        [Required]
        public CriteriaDto Criteria { get; set; }

        [Required]
        public int[] ReviewerIDs { get; set; }

        public int[] ValidatorIDs { get; set; }

        [Required]
        public Distribution DistributionRule { get; set; }

        //The fields that can only be seen in adddition to the editable fields.
        public FieldType[] VisibleFields { get; set; }
    }
}