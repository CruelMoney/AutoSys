#region Using

using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Storage.Repository;

#endregion

namespace StudyConfigurationServer.Models
{
    /// <summary>
    ///     This class defines a bibliographic item.
    /// </summary>
    public class Item : IEntity
    {
        /// <summary>
        ///     Type of a bibliographic item.
        /// </summary>
        public enum ItemType
        {
            Article,
            Book,
            Booklet,
            Misc,
            Conference,
            InBook,
            InCollection,
            InProceedings,
            PhDThesis,
            Proceedings,
            Techreport,
            Unpublished,
            Manual
        }

        /// <summary>
        ///     The type of this bibliographic item. (e.g., Article, Book, etc.)
        /// </summary>
        public readonly ItemType Type;

        public Item(ItemType type, IDictionary<FieldType, string> fields)
        {
            Type = type;
            FieldKeys = fields.Keys.ToList();
            FieldValues = new List<StoredString>();
            fields.Values.ForEach(s => FieldValues.Add(new StoredString {Value = s}));
        }

        public Item()
        {
            FieldValues = new List<StoredString>();
        }

        public virtual ICollection<FieldType> FieldKeys { get; set; }
        public virtual List<StoredString> FieldValues { get; set; }

        public ICollection<StudyTask> Tasks { get; set; }

        public int ID { get; set; }

        public string FindFieldValue(string fieldType)
        {
            //Find the index of fieldType
            var fieldIndex = FieldKeys.ToList().
                FindIndex(t => t.Type.ToString().
                    Equals(fieldType));

            //Return the matchin value
            return fieldIndex != -1
                ? FieldValues.ToList()[fieldIndex].Value
                : null;
        }
    }
}