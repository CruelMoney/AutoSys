using System.Collections.Generic;

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.CriteriaValidation
{
    public interface IRuleChecker
    {
        /// <summary>
        /// Checks whether the given data meets the rule.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="criteriaData"></param>
        /// <returns>true when the specified item is valid; false otherwise.</returns>
        bool IsRuleMet(ICollection<string> data, ICollection<string> criteriaData);
    }
}
