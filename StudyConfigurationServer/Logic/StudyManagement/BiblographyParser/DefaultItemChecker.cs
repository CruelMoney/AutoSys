#region Using

using System.Linq;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyManagement.BiblographyParser
{
    /// <summary>
    ///     Default <see cref="IItemChecker" /> implementation for when no custom checker is specified.
    ///     When all fields contained by the item are valid, the item is valid.
    /// </summary>
    public class DefaultItemChecker : IItemChecker
    {
        private readonly FieldValidator _validator = new FieldValidator();

        public bool Validate(Item item)
        {
            for (var i = 0; i < item.FieldKeys.Count; i++)
            {
                var key = item.FieldKeys.ToList()[i];
                var value = item.FieldValues.ToList()[i];

                if (!item.FieldKeys.All(field => _validator.IsFieldValid(value.Value, key)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}