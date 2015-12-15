#region Using

using System.Collections.Generic;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser
{
    // <author>Jacob Cholewa</author>
    /// <summary>
    ///     Parses text containing bibliographic data into a collection of bibliography <see cref="Item" /> objects.
    /// </summary>
    public interface IBibliographyParser
    {
        List<Item> Parse(string data);
    }
}