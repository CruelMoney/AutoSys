using System.Linq;
using Logic.Model;

namespace Logic.StudyConfiguration.BiblographyParser
{
    /// <summary>
    /// Default <see cref="IItemChecker"/> implementation for when no custom checker is specified.
    /// When all fields contained by the item are valid, the item is valid.
    /// </summary>
    public class DefaultItemChecker : IItemChecker
    {
        readonly FieldValidator _validator = new FieldValidator();

        public bool Validate(ItemLogic item)
        {
            return item.Fields.All(field => _validator.IsFieldValid(field.Value, field.Key));
        }

    }
}
