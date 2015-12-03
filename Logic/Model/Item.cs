﻿using System.Collections.Generic;

namespace Logic.Model
{
    /// <summary>
    ///     This class defines a bibliographic item.
    /// </summary>
    public class Item
    {
        /// <summary>
        ///     Type of a Field associated to a bibliographic item.
        /// </summary>
        public enum FieldType
        {
            Address,
            Annote,
            Author,
            Booktitle,
            Chapter,
            Crossref,
            Edition,
            Editor,
            HowPublished,
            Instritution,
            Journal,
            Key,
            Month,
            Note,
            Number,
            Organization,
            Pages,
            Publisher,
            School,
            Series,
            Title,
            Type,
            Volume,
            Year,
            URL,
            ISBN,
            ISSN,
            LCCN,
            Abstract,
            Keywords,
            Price,
            Copyright,
            Language,
            Contents,
            Doi
        }

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
        ///     A collection of fields and associated values, contained within this bibliographic item.
        /// </summary>
        public readonly IReadOnlyDictionary<FieldType, string> Fields;

        /// <summary>
        ///     The type of this bibliographic item. (e.g., Article, Book, etc.)
        /// </summary>
        public readonly ItemType Type;

        public Item(ItemType type, IDictionary<FieldType, string> fields)
        {
            Type = type;
            Fields = new Dictionary<FieldType, string>(fields);
        }
    }
}