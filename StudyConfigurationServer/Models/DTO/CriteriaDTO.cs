using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudyConfigurationServer.Models.DTO
{
    public class CriteriaDTO
    {
        public CriteriaDTO(Criteria criteria)
        {
            Id = criteria.ID;
            Name = criteria.Name;
            DataType = (DataFieldDTO.DataType)Enum.Parse(typeof(DataField.DataType), criteria.DataType.ToString());
            Description = criteria.Description;
            if (criteria.TypeInfo != null){TypeInfo = criteria.TypeInfo.Select(s => s.Value).ToArray();}
            if (criteria.DataMatch != null){DataMatch = criteria.DataMatch.Select(s => s.Value).ToArray();}
            Rule = (CriteriaRule)Enum.Parse(typeof(Criteria.CriteriaRule), criteria.Rule.ToString()); ;

        }
        public enum CriteriaRule
        {
            Contains,
            Equals,
            LargerThan,
            SmallerThan,
            BeforeYear,
            AfterYear,
            IsYear,
            Exists
        }

        public int Id { get; set; }

        /// <summary>
        /// A name for the criteria.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description for the criteria, so the User understands what data is requested.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of data this criteria holds.
        /// </summary>
        public DataFieldDTO.DataType DataType { get; set; }

        /// <summary>
        /// For <see cref="DataField.DataType.Enumeration"/> and <see cref="DataField.DataType.Flags"/> data types, a collection of the predefined values.
        /// </summary>
        public string[] TypeInfo { get; set; }

        /// <summary>
        /// The data the rule is checked against. 
        /// The data this Field holds depends on the data type.
        /// For all but <see cref="DataField.DataType.Flags" /> this array contains just one element; the representation of the object for that data type (see <see cref="DataType" />).
        /// For DataField it can contain several flags that is checked in regards to the rule. 
        /// </summary>
        public string[] DataMatch { get; set; }

        /// <summary>
        /// A rule for when the criteria is met / true. 
        /// </summary>
        public CriteriaRule Rule { get; set; }

        public CriteriaDTO()
        {
        }
    }
}