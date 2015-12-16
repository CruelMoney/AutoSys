#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement.TaskValidation
{
    /// <summary>
    /// The default criteriaChecker for Choosing a ruleChecker based on a rule enum.
    /// </summary>
    public class DefaultCriteriaChecker : ICriteriaChecker
    {
        private readonly Dictionary<Criteria.CriteriaRule, IRuleChecker> _checkers;
        private readonly IRuleChecker _defaultRuleChecker = new DefaultRuleChecker();

        public DefaultCriteriaChecker(Dictionary<Criteria.CriteriaRule, IRuleChecker> checkers = null)
        {
            _checkers = checkers ?? new Dictionary<Criteria.CriteriaRule, IRuleChecker>
            {
                {Criteria.CriteriaRule.Exists, new DataExistsRule()},
                {Criteria.CriteriaRule.Contains, new DataContainsRule()},
                {Criteria.CriteriaRule.Equals, new EqualIgnoreOrderRule()},
                {Criteria.CriteriaRule.AfterDate, new AfterDateRule()},
                {Criteria.CriteriaRule.BeforeDate, new BeforeDateRule()},
                {Criteria.CriteriaRule.IsYear, new IsYear()},
                {Criteria.CriteriaRule.LargerThan, new LargerThanRule()},
                {Criteria.CriteriaRule.SmallerThan, new SmallerThanRule()}
            };
        }

        /// <summary>
        /// Validate the data using the criterions rules corresponding ruleChekcer.  
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool Validate(Criteria criteria, string[] data)
        {
            var type = criteria.Rule;
            var checkData = data;
            var criteriaData = criteria.DataMatch.Select(s => s.Value).ToArray();
            if (_checkers.ContainsKey(type))
            {
                return _checkers[type].IsRuleMet(checkData, criteriaData);
            }
            throw new NotImplementedException("No RuleChecker exists for this rule");
        }
    }
}