﻿namespace Logic.Model.CriteriaValidator
{
    public interface ICriteriaChecker
    {
        /// <summary>
        /// Checks whether a specified criteria is met.
        /// </summary>
        /// <param name="criteria">The item to Validate.</param>
        /// <returns>true when the specified item is valid; false otherwise.</returns>
        bool Validate(Criteria criteria, DataFieldLogic data);
    }
}
