﻿#region Using

using System.Collections.Generic;
using Storage.Repository;

#endregion

namespace StudyConfigurationServer.Models
{
    public class Criteria : IEntity
    {
        public enum CriteriaRule
        {
            Contains,
            Equals,
            LargerThan,
            SmallerThan,
            BeforeDate,
            AfterDate,  
            IsYear,
            Exists
        }

        /// <summary>
        ///     A name for the criteria.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     A description for the criteria, so the User understands what data is requested.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The type of data this criteria holds.
        /// </summary>
        public DataField.DataType DataType { get; set; }

        /// <summary>
        ///     For <see cref="DataField.DataType.Enumeration" /> and <see cref="DataField.DataType.Flags" /> data types, a
        ///     collection of the predefined values.
        /// </summary>
        public virtual ICollection<StoredString> TypeInfo { get; set; }

        /// <summary>
        ///     The data the rule is checked against.
        ///     The data this Field holds depends on the data type.
        ///     For all but <see cref="DataField.DataType.Flags" /> this array contains just one element; the representation of the
        ///     object for that data type (see <see cref="DataType" />).
        ///     For DataField it can contain several flags that is checked in regards to the rule.
        /// </summary>
        public virtual ICollection<StoredString> DataMatch { get; set; }

        /// <summary>
        ///     A rule for when the criteria is met / true.
        /// </summary>
        public CriteriaRule Rule { get; set; }

        public int ID { get; set; }
    }
}
