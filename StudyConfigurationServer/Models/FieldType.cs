#region Using

using System;
using Storage.Repository;

#endregion

namespace StudyConfigurationServer.Models
{
    public class FieldType : IEntity
    {
        public enum TypEField
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
            Url,
            Isbn,
            Issn,
            Lccn,
            Abstract,
            Keywords,
            Price,
            Copyright,
            Language,
            Contents,
            Doi
        }

        public FieldType(string fieldType)
        {
            Type = (TypEField) Enum.Parse(typeof (TypEField), fieldType, true);
        }

        public FieldType()
        {
        }

        public TypEField Type { get; set; }


        public int ID { get; set; }
    }
}