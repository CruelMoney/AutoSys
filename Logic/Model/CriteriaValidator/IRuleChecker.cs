namespace Logic.Model.CriteriaValidator
{
    public interface IRuleChecker
    {
        /// <summary>
        /// Checks whether the given data meets the rule.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>true when the specified item is valid; false otherwise.</returns>
        bool IsRuleMet(DataFieldLogic data, Criteria criteria);
    }
}
