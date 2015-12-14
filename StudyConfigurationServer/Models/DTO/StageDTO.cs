using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyConfigurationServer.Models.DTO
{
    public class StageDTO
    {
        public StageDTO(Stage stage)
        {
            Name = stage.Name;
            Id = stage.ID;
            Criteria = new CriteriaDTO(stage.Criteria.ElementAt(0));
            StudyID = stage.Study.ID;
            ReviewerIDs = (from u in stage.Users where u.StudyRole == UserStudies.Role.Reviewer select u.ID).ToArray();
            ValidatorIDs = (from u in stage.Users where u.StudyRole == UserStudies.Role.Validator select u.ID).ToArray();
            DistributionRule = (Distribution)Enum.Parse(typeof(Stage.Distribution), stage.DistributionRule.ToString());
            VisibleFields =  stage.VisibleFields.Select(vf => (FieldType) Enum.Parse(typeof (StudyConfigurationServer.Models.FieldType.TypEField), vf.Type.ToString())).ToArray();
            
        }
        public StageDTO()
        {
        }
        public string Name { get; set; }
        public int Id { get; set; }
        //The critderia are defining what fields are going to be editable for this stage
        [Required]
        public CriteriaDTO Criteria { get; set; }
        public int StudyID { get; set; }
        [Required]
        public int[] ReviewerIDs { get; set; }
        public int[] ValidatorIDs { get; set; }
        [Required]
        public Distribution DistributionRule { get; set; }
        //The fields that can only be seen in adddition to the editable fields.
        public FieldType[] VisibleFields { get; set; }

        public enum Distribution
        {
            FiftyPercentOverlap,
            HundredPercentOverlap,
            NoOverlap,
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
            URL,
            ISBN,
            ISSN,
            LCCN,
            Abstract,
            Keywords,
            Price,
            Copyright,
            Language,
            Contents,
            Doi,
        }
    }
}