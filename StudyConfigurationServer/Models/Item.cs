using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    /// <summary>
    ///     This class defines a bibliographic item.
    /// </summary>
    public class Item :IEntity
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

        public virtual ICollection<FieldType> fieldKeys { get; set; }
        public virtual List<StoredString> fieldValues { get; set; }

        /// <summary>
        ///     The type of this bibliographic item. (e.g., Article, Book, etc.)
        /// </summary>
        public readonly ItemType Type;

        public Item(ItemType type, IDictionary<FieldType, string> fields)
        {
            Type = type;
            fieldKeys = fields.Keys.ToList();
            fieldValues = new List<StoredString>();
            fields.Values.ForEach(s => fieldValues.Add(new StoredString() {Value = s}));
        }

        public int ID { get; set; }

        public ICollection<StudyTask> Tasks { get; set; }

        public Item()
        {
            fieldValues = new List<StoredString>();
        }

        public string FindFieldValue(string fieldType)
        {
            //Find the index of fieldType
            var fieldIndex = fieldKeys.ToList().
                FindIndex(t => t.Type.ToString().
                Equals(fieldType));

            //Return the matchin value
            return fieldIndex != -1 ? 
                fieldValues.ToList()[fieldIndex].Value : null;
        }
    }
}