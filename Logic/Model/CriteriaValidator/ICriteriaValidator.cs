namespace Logic.Model.CriteriaValidator
{
    public interface ICriteriaValidator
    {
        /// <summary>
        /// Checks whether a specified criteria is met.
        /// </summary>
        /// <param name="criteria">The item to Validate.</param>
        /// <returns>true when the specified item is valid; false otherwise.</returns>
        bool CriteriaIsMet(Criteria criteria, DataFieldLogic data);
    }
}
