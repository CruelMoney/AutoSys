using System.Collections.Generic;
using Logic.Model;
using Logic.StudyConfiguration.BiblographyParser;

namespace BibliographyParser
{
    /// <summary>
    /// This class is used for validating bibliographic <see cref="ItemLogic"/> objects.
    /// </summary>
    public class ItemValidator
    {
        readonly Dictionary<ItemLogic.ItemType, IItemChecker> _checkers;
        readonly IItemChecker _defaultChecker = new DefaultItemChecker();

        /// <summary>
        /// Constructs a new ItemValidator.
        /// </summary>
        /// <param name="checkers">
        /// A dictionary of Field checkers per item type.
        /// If a checker for an item type is not specified, <see cref="DefaultFieldChecker"/> is used.
        /// </param>
        public ItemValidator(Dictionary<ItemLogic.ItemType, IItemChecker> checkers = null)
        {
            _checkers = checkers ?? new Dictionary<ItemLogic.ItemType, IItemChecker>();
        }

        /// <summary>
        /// Checks whether or not a given item is valid.
        /// </summary>
        /// <param name="item">The item to validate.</param>
        /// <returns>true if the item is valid; false otherwise.</returns>
        public bool IsItemValid(ItemLogic item)
        {
            return _checkers.ContainsKey(item.Type) ? _checkers[item.Type].Validate(item) : _defaultChecker.Validate(item);
        }
    }
}
