#region Using

using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement.TaskValidation
{
    public interface ICriteriaValidator
    {
        /// <summary>
        ///     Checks whether a specified criteria is met.
        /// </summary>
        /// <param name="criteria">The item to Validate.</param>
        /// <param name="data"></param>
        /// <returns>true when the specified item is valid; false otherwise.</returns>
        bool CriteriaIsMet(Criteria criteria, string[] data);
    }
}