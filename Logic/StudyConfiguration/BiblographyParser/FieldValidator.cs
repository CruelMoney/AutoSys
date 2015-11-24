using System;
using System.Collections.Generic;
using Logic.Model;
using Logic.StudyConfiguration.BiblographyParser;

namespace BibliographyParser
{
    /// <summary>
    /// Class which validates fields associated to bibliographic items.
    /// </summary>
    public class FieldValidator
    {
        readonly Dictionary<Item.FieldType, IFieldChecker> _checkers;
        readonly IFieldChecker _defaultChecker = new DefaultFieldChecker();

        /// <summary>
        /// Constructs a new <see cref="FieldValidator"/>.
        /// </summary>
        /// <param name="checkers">A dictionary of Field checkers. If not specified, <see cref="DefaultFieldChecker"/> is used.</param>
        public FieldValidator(Dictionary<Item.FieldType, IFieldChecker> checkers = null)
        {
            _checkers = checkers ?? new Dictionary<Item.FieldType, IFieldChecker>();
        }

        /// <summary>
        /// Checks whether or not a given Field is valid.
        /// </summary>
        /// <param name="field">The Field data to validate.</param>
        /// <param name="type">The Field type.</param>
        /// <returns>returns true if the Field is valid; false otherwise.</returns>
        public bool IsFieldValid(string field, Item.FieldType type)
        {
            return _checkers.ContainsKey(type) ? _checkers[type].Validate(field) : _defaultChecker.Validate(field);
        }
    }
}