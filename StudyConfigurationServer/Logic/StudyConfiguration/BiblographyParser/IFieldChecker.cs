namespace StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser
{
    /// <summary>
    /// Interface for Field checkers.
    /// </summary>
    public interface IFieldChecker
    {
        /// <summary>
        /// Checks whether a specified Field is valid.
        /// </summary>
        /// <param name="field">The Field to Validate.</param>
        /// <returns>true when the specified Field is valid; false otherwise.</returns>
        bool Validate(string field);
    }
}
