using System.Collections.Generic;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser
{
    /// <summary>
    /// Parses text containing bibliographic data into a collection of bibliography <see cref="Item"/> objects.
    /// </summary>
    public interface IBibliographyParser
    {
        List<Item> Parse(string data);
    }
}
