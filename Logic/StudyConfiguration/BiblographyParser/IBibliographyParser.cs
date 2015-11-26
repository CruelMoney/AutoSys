using System.Collections.Generic;
using Logic.Model;

namespace BibliographyParser
{
    /// <summary>
    /// Parses text containing bibliographic data into a collection of bibliography <see cref="ItemLogic"/> objects.
    /// </summary>
    public interface IBibliographyParser
    {
        List<ItemLogic> Parse(string data);
    }
}
