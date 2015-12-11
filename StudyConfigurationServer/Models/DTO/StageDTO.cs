using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudyConfigurationServer.Models.DTO
{
    public class StageDTO
    {
        public string Name { get; set; }
        public int Id { get; set; }
        //The criteria are defining what fields are going to be editable for this stage
        [Required]
        public Criteria[] Criteria { get; set; }
        public int StudyID { get; set; }
        //The fields that can only be seen in adddition to the editable fields.
        public Item.FieldType[] VisibleFields { get; set; }
        [Required]
        public int[] ReviewerIDs { get; set; }
        public int[] ValidatorIDs { get; set; }
        [Required]
        public Stage.Distribution DistributionRule { get; set; }
        
    }
}