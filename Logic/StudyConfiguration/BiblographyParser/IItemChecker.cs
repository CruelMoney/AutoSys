

using Logic.Data;

namespace Logic.StudyConfiguration.BiblographyParser
{
    /// <summary>
    /// Interface for Field checkers.
    /// </summary>
    public interface IItemChecker
    {
        /// <summary>
        /// Checks whether a specified item is valid.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>true when the specified item is valid; false otherwise.</returns>
        bool Validate(Item item);
    }
}
