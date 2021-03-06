﻿using System.ComponentModel.DataAnnotations;

namespace StudyConfigurationUILibrary.Data
{
    public class StageDTO
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
            Doi
        }

        public string Name { get; set; }
        public int Id { get; set; }
        //The criteria are defining what fields are going to be editable for this stage
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
    }
}