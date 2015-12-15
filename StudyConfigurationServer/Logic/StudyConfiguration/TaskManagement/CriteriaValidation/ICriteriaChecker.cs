#region Using

using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.CriteriaValidation
{
    public interface ICriteriaChecker
    {
        /// <summary>
        ///     Checks whether a specified criteria is met.
        /// </summary>
        /// <param name="criteria">The item to Validate.</param>
        /// <returns>true when the specified item is valid; false otherwise.</returns>
        bool Validate(Criteria criteria, string[] data);
    }
}