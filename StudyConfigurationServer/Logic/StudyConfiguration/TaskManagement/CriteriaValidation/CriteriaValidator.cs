#region Using

using System.Collections.Generic;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.CriteriaValidation
{
    public class CriteriaValidator : ICriteriaValidator
    {
        private readonly Dictionary<DataField.DataType, ICriteriaChecker> _checkers;
        private readonly ICriteriaChecker _defaultChecker = new DefaultCriteriaChecker();

        /// <summary>
        ///     Constructs a new <see cref="FieldValidator" />.
        /// </summary>
        /// <param name="checkers">A dictionary of Field checkers. If not specified, <see cref="DefaultFieldChecker" /> is used.</param>
        public CriteriaValidator(Dictionary<DataField.DataType, ICriteriaChecker> checkers = null)
        {
            _checkers = checkers ?? new Dictionary<DataField.DataType, ICriteriaChecker>();
        }

        /// <summary>
        ///     Checks whether or not a given criteria is met.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="data"></param>
        /// <returns>returns true if the Field is valid; false otherwise.</returns>
        public bool CriteriaIsMet(Criteria criteria, string[] data)
        {
            var type = criteria.DataType;
            return _checkers.ContainsKey(type)
                ? _checkers[type].Validate(criteria, data)
                : _defaultChecker.Validate(criteria, data);
        }
    }
}