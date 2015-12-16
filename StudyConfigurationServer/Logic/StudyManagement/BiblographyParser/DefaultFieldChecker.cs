#region Using

using System.Text.RegularExpressions;

#endregion

namespace StudyConfigurationServer.Logic.StudyManagement.BiblographyParser
{
    // <author>Jacob Cholewa</author>
    /// <summary>
    ///     Default <see cref="IFieldChecker" /> implementation. Matches all strings that do not contain newlines.
    /// </summary>
    public class DefaultFieldChecker : IFieldChecker
    {
        private readonly Regex _r = new Regex("^.*$");

        public bool Validate(string s)
        {
            return _r.IsMatch(s);
        }
    }
}