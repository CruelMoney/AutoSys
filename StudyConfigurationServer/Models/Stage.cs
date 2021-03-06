﻿#region Using

using System.Collections.Generic;
using Storage.Repository;

#endregion

namespace StudyConfigurationServer.Models
{
    public class Stage : IEntity
    {
        public enum Distribution
        {
            FiftyPercentOverlap,
            HundredPercentOverlap,
            NoOverlap
        }

        public Stage()
        {
            VisibleFields = new List<FieldType>();
        }

        public string Name { get; set; }

        //The criteria are defining what fields are editable for this stage
        public virtual List<Criteria> Criteria { get; set; }

        public List<StudyTask> Tasks { get; set; }
        //The fields that can only be seen in adddition to the editable fields.

        public virtual List<FieldType> VisibleFields { get; set; }

        public virtual ICollection<UserStudies> Users { get; set; }

        public Study Study { get; set; }

        /// <summary>
        ///     Defines wether the stage is currently reviewing or validating.
        /// </summary>
        public StudyTask.Type CurrentTaskType { get; set; }

        public Distribution DistributionRule { get; set; }

        public bool IsCurrentStage { get; set; }
        public int ID { get; set; }
    }
}