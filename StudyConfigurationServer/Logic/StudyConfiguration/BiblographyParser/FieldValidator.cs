#region Using

using System.Collections.Generic;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser
{
    // <author>Jacob Cholewa</author>
    /// <summary>
    ///     Class which validates fields associated to bibliographic items.
    /// </summary>
    public class FieldValidator
    {
        private readonly Dictionary<FieldType, IFieldChecker> _checkers;
        private readonly IFieldChecker _defaultChecker = new DefaultFieldChecker();

        /// <summary>
        ///     Constructs a new <see cref="FieldValidator" />.
        /// </summary>
        /// <param name="checkers">A dictionary of Field checkers. If not specified, <see cref="DefaultFieldChecker" /> is used.</param>
        public FieldValidator(Dictionary<FieldType, IFieldChecker> checkers = null)
        {
            _checkers = checkers ?? new Dictionary<FieldType, IFieldChecker>();
        }

        /// <summary>
        ///     Checks whether or not a given Field is valid.
        /// </summary>
        /// <param name="field">The Field data to Validate.</param>
        /// <param name="type">The Field type.</param>
        /// <returns>returns true if the Field is valid; false otherwise.</returns>
        public bool IsFieldValid(string field, FieldType type)
        {
            return _checkers.ContainsKey(type) ? _checkers[type].Validate(field) : _defaultChecker.Validate(field);
        }
    }
}