using System.Linq;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser
{
    /// <summary>
    /// Default <see cref="IItemChecker"/> implementation for when no custom checker is specified.
    /// When all fields contained by the item are valid, the item is valid.
    /// </summary>
    public class DefaultItemChecker : IItemChecker
    {
        readonly FieldValidator _validator = new FieldValidator();

        public bool Validate(Item item)
        {
            
            for (int i =0; i<item.fieldKeys.Count; i++)
            {
                var key = item.fieldKeys.ToList()[i];
                var value = item.fieldValues.ToList()[i];

                if (!item.fieldKeys.All(field => _validator.IsFieldValid(value.Value, key)))
                {
                    return false;
                }
              
            }
            return true;

        }

    }
}
