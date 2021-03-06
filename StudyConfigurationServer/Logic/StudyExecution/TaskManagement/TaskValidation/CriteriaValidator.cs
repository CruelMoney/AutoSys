#region Using

using System.Collections.Generic;
using StudyConfigurationServer.Logic.StudyManagement.BiblographyParser;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement.TaskValidation
{
    /// <summary>
    /// A CriteriaValidator for validating data based on a criteria.
    /// The Class will choose a criteriaChecker based on the criterions fieldType.
    /// This allows for different RuleCheckers for different fieldTypes. 
    /// Example: a enummeration have a different criteriaChecker than a string, thus having different ruleChekcers for fieldTypes. 
    /// </summary>
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
        ///     Choosing a criteriaChecker based on the criterions datatype. 
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