﻿#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Storage.Repository;
using StudyConfigurationServer.Api;

#endregion

namespace StudyConfigurationServer.Models
{
    public class DataField : IEntity
    {
        /// <summary>
        ///     Defines the type of data the data Field holds.
        /// </summary>
        public enum DataType
        {
            String,

            /// <summary>
            ///     Either "True", or "False".
            /// </summary>
            Boolean,

            /// <summary>
            ///     Select one out of a list of predefined values in <see cref="TypeInfo" />.
            /// </summary>
            Enumeration,

            /// <summary>
            ///     Select multiple or none out of a list of predefined values in <see cref="TypeInfo" />.
            ///     In addition, new entries by the User are allowed.
            /// </summary>
            Flags,

            /// <summary>
            ///     A link to a resource which can be obtained by calling <see cref="StudyController.GetResource" />.
            ///     The information is encoded as a JSON string representing <see cref="Resource" />.
            /// </summary>
            Resource
        }

        /// <summary>
        ///     A name for the data Field.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     A description for the data Field, so the User understands what data is requested.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The type of data this data Field holds.
        /// </summary>
        public DataType FieldType { get; set; }

        /// <summary>
        ///     For <see cref="DataType.Enumeration" /> and <see cref="DataField.DataType.Flags" /> data types, a
        ///     collection of the predefined values.
        /// </summary>
        public ICollection<StoredString> TypeInfo { get; set; }

        /// <summary>
        ///     The list of data entered by users.
        /// </summary>
        public virtual List<UserData> UserData { get; set; }

        /// <summary>
        ///     A list of conflicting userData in case of a <see cref="StudyTask.Type.Conflict" /> task.
        ///     The user is meant to choose one and copy it to the UserData list as the answer.
        /// </summary>
        public virtual List<UserData> ConflictingData { get; set; }

        public int ID { get; set; }

        public DataField SubmitData(int userId, string[] data)
        {
            //We expect that the user only exists once per dataField
            UserData dataToUpdate;

            try
            {
                dataToUpdate = UserData.First(d => d.UserId == userId);
            }
            catch (Exception)
            {
                throw new ArgumentException("User not associated with task");
            }

            dataToUpdate.Data.Clear();
            foreach (var s in data)
            {
                dataToUpdate.Data.Add(new StoredString {Value = s});
            }


            return this;
        }

        /// <summary>
        ///     Returns true the given user has entered data for this field. If no userID is given
        ///     it checks for all users associated with this field.
        /// </summary>
        /// <param name="userId">
        ///     The user to check if has entered data</ram>
        ///     <returns></returns>
        public bool DataEntered(int? userId = null)
        {
            if (userId == null)
            {
                return UserData.TrueForAll(d => d.ContainsData());
            }
            try
            {
                return UserData.First(u => u.UserId == userId).ContainsData();
            }
            catch (Exception)
            {
                throw new ArgumentException("The user is not associated with this task");
            }
        }

        public bool UserDataIsConflicting()
        {
            var data = UserData.Select(d => d.Data.Select(s => s.Value).ToList()).ToList();

            return data.Any(d => !d.SequenceEqual(data.First()));
        }
    }
}