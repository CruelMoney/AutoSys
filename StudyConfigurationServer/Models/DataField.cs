using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Ajax.Utilities;
using Storage.Repository;
using StudyConfigurationServer.Api;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Models
{
    public class DataField : IEntity
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
            /// In addition, new entries by the User are allowed.
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
        /// A description for the data Field, so the User understands what data is requested.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of data this data Field holds.
        /// </summary>
        public DataType FieldType { get; set; }
       
        /// <summary>
        /// For <see cref="DataField.DataType.Enumeration"/> and <see cref="DataField.DataType.Flags"/> data types, a collection of the predefined values.
        /// </summary>
        public string[] TypeInfo { get; set; }

        /// <summary>
        /// </summary>
        public List<UserData> UserData { get; set; }

        public int Id { get; set; }

        public DataField SubmitData(int userId, string[] data)
        {
            //We expect that the user only exists once per dataField
            var dataToUpdate = UserData.First(d => d.User.Id == userId);

            if (dataToUpdate==null)
            {
                throw new ArgumentException("User not associated with task");
            }

            dataToUpdate.Data = data;

            return this;
        }

        public bool DataEntered(int? userID = null )
        {
                return UserData.TrueForAll(d => d.ContainsData());
        }
    }
}
