using Logic.Controllers;
using StudyConfigurationServer.Api;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Models
{
    public class DataField
    {
        /// <summary>
        /// Defines the type of data the data Field holds.
        /// </summary>
        public enum DataType
        {
            String,
            /// <summary>
            /// Either "True", or "False".
            /// </summary>
            Boolean,
            /// <summary>
            /// Select one out of a list of predefined values in <see cref="TypeInfo" />.
            /// </summary>
            Enumeration,
            /// <summary>
            /// Select multiple or none out of a list of predefined values in <see cref="TypeInfo"/>.
            /// In addition, new entries by the user are allowed.
            /// </summary>
            Flags,
            /// <summary>
            /// A link to a resource which can be obtained by calling <see cref="StudyController.GetResource" />.
            /// The information is encoded as a JSON string representing <see cref="Resource" />.
            /// </summary>
            Resource
        }

        /// <summary>
        /// A name for the data Field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description for the data Field, so the user understands what data is requested.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of data this data Field holds.
        /// </summary>
        public DataType FieldType { get; set; }

        /// <summary>
        /// For <see cref="DataType.Enumeration"/> and <see cref="DataType.Flags"/> data types, a collection of the predefined values.
        /// </summary>
        public string[] TypeInfo { get; set; }

        /// <summary>
        /// This property holds the data for the Field and can be used to provide default data to the user, as well as by the user to submit the StudyTask.
        /// The data this Field holds depends on the data type.
        /// For all but <see cref="DataType.Flags" /> this array contains just one element; the representation of the object for that data type (see <see cref="DataType" />).
        /// For <see cref="DataType.Flags" /> it can contain several flags, either existing ones listed in <see cref="TypeInfo" />, or new ones.
        /// For <see cref="DataType.Resource" /> it contains a JSON representation of <see cref="ResourceDTO" />.
        /// </summary>
        public string[] Data { get; set; }
    }
}
