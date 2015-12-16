#region Using

using System.Collections.Generic;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyManagement.BiblographyParser
{
    // <author>Jacob Cholewa</author>
    /// <summary>
    ///     This class is used for validating bibliographic <see cref="Item" /> objects.
    /// </summary>
    public class ItemValidator
    {
        private readonly Dictionary<Item.ItemType, IItemChecker> _checkers;
        private readonly IItemChecker _defaultChecker = new DefaultItemChecker();

        /// <summary>
        ///     Constructs a new ItemValidator.
        /// </summary>
        /// <param name="checkers">
        ///     A dictionary of Field checkers per item type.
        ///     If a checker for an item type is not specified, <see cref="DefaultFieldChecker" /> is used.
        /// </param>
        public ItemValidator(Dictionary<Item.ItemType, IItemChecker> checkers = null)
        {
            _checkers = checkers ?? new Dictionary<Item.ItemType, IItemChecker>();
        }

        /// <summary>
        ///     Checks whether or not a given item is valid.
        /// </summary>
        /// <param name="item">The item to Validate.</param>
        /// <returns>true if the item is valid; false otherwise.</returns>
        public bool IsItemValid(Item item)
        {
            return _checkers.ContainsKey(item.Type)
                ? _checkers[item.Type].Validate(item)
                : _defaultChecker.Validate(item);
        }
    }
}