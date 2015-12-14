using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Storage.Repository;

namespace StudyConfigurationServer.Models
{
    public class FieldType :IEntity
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


        public int ID { get; set; }

        public TypEField Type { get; set; }

        public FieldType(string fieldType)
        {
            Type = (TypEField) Enum.Parse(typeof (TypEField), fieldType, true);
        }

        public FieldType()
        {
           
        }
    }
}