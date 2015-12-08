using System;
using System.Collections.Generic;

namespace StudyConfigurationUI.Data.CriteriaValidator
{
    public class DefaultCriteriaChecker : ICriteriaChecker
    {
        private readonly Dictionary<Criteria.CriteriaRule, IRuleChecker> _checkers;
        private readonly IRuleChecker _defaultRuleChecker = new DefaultRuleChecker();

        public DefaultCriteriaChecker(Dictionary<Criteria.CriteriaRule, IRuleChecker> checkers = null)
        {
            _checkers = checkers ?? new Dictionary<Criteria.CriteriaRule, IRuleChecker>()
            {
                {Criteria.CriteriaRule.Exists, new DataExistsRule()},
                {Criteria.CriteriaRule.Contains, new DataContainsRule()},
                {Criteria.CriteriaRule.Equals, new EqualIgnoreOrderRule()},
                {Criteria.CriteriaRule.AfterYear, new AfterYearRule()},
                {Criteria.CriteriaRule.BeforeYear, new BeforeYearRule()},
                {Criteria.CriteriaRule.IsYear, new IsYear()},
                {Criteria.CriteriaRule.LargerThan, new LargerThanRule()},
                {Criteria.CriteriaRule.SmallerThan, new SmallerThanRule()},
            };
        }

        public bool Validate(Criteria criteria, DataField data)
        {
            var type = criteria.Rule;
            var checkData = data.Data;
            var criteriaData = criteria.DataMatch;
            if (_checkers.ContainsKey(type))
            {
                return _checkers[type].IsRuleMet(checkData, criteriaData);
            }
            else
            {
                throw new ArgumentException("No RuleChecker exists for this rule");
            }
        }

    }
}
