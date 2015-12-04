using Storage.Repository;

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
            BeforeYear,
            AfterYear,
            IsYear,
            Exists
        }

        public virtual Stage Stage { get; set; } // reference to Stage (many to one)

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
        public DataField.DataType DataType { get; set; }

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
        
    }
}
