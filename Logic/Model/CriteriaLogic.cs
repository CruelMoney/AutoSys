using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model.DTO;
using Newtonsoft.Json.Bson;
using Storage.Repository;

namespace Logic.Model
{
    public class CriteriaLogic : IEntity
    {    
        public enum CriteriaType
        {
            Contains,
            Equals,
            LargerThan,
            SmallerThan,
            BeforeYear,
            AfterYear,
            IsYear,
            Exists
        }

        public int Id { get; set; }

        /// <summary>
        /// A name for the criteria.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// A description for the criteria, so the user understands what data is requested.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The type of data this criteria holds.
        /// </summary>
        public DataField.DataType DataType { get; set; }

        /// <summary>
        /// The data the rule is checked against. 
        /// The data this Field holds depends on the data type.
        /// For all but <see cref="DataField.DataType.Flags" /> this array contains just one element; the representation of the object for that data type (see <see cref="DataType" />).
        /// For DataField it can contain several flags that is checked in regards to the rule. 
        /// </summary>
        public string[] DataMatch { get; set; }

        /// <summary>
        /// A rule for when the criteria is met / true. 
        /// </summary>
        public CriteriaType Rule { get; set; }

        /// <summary>
        /// Takes a <see cref="DataField" /> and checks the Datafields data against the criterias DataMatch using the defined rule. 
        /// </summary>
        /// <param name="data">The data to check if meets the rule.</param>
        public bool CriteriaIsMet(DataField data)
        {
            switch (data.FieldType)
            {
                case DataField.DataType.String:
                    return CheckString(data);
                case DataField.DataType.Boolean:
                    return CheckBool(data);
                case DataField.DataType.Enumeration:
                    return CheckEnum(data);
                case DataField.DataType.Flags:
                    return CheckFlags(data);
                case DataField.DataType.Resource:
                    return CheckResource(data);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        ///TODO Maybe check if the source is avaliable. Don't know excactly how sources work.
        private bool CheckResource(DataField data)
        {
            switch (Rule)
            {
                case CriteriaType.Exists:
                    return data.Data.Any(); 
                default:
                    throw new ArgumentOutOfRangeException("The criteria rule doesn't match the dataType");
            }
        }

        private bool CheckFlags(DataField data)
        {
            switch (Rule)
            {
                case CriteriaType.Contains:
                    return !DataMatch.Except(data.Data).Any();
                case CriteriaType.Equals:
                    return DataMatch.SequenceEqual(data.Data);
                case CriteriaType.LargerThan:
                    return data.Data.Length > DataMatch.Length;
                case CriteriaType.SmallerThan:
                    return data.Data.Length < DataMatch.Length;
                case CriteriaType.Exists:
                    return data.Data.Any();
                default:
                    throw new ArgumentOutOfRangeException("The criteria rule doesn't match the dataType");
            }
        }

        private bool CheckEnum(DataField data)
        {
            switch (Rule)
            {
                case CriteriaType.Equals:
                    return DataMatch.SequenceEqual(data.Data);
                case CriteriaType.LargerThan:
                    return data.Data.Length > DataMatch.Length;
                case CriteriaType.SmallerThan:
                    return data.Data.Length < DataMatch.Length;
                case CriteriaType.Exists:
                    return data.Data.Any();
                default:
                    throw new ArgumentOutOfRangeException("The criteria rule doesn't match the dataType");
            }
        }

        private bool CheckBool(DataField data)
        {
            switch (Rule)
            {
                case CriteriaType.Equals:
                    return DataMatch.SequenceEqual(data.Data);
               default:
                    throw new ArgumentOutOfRangeException("The criteria rule doesn't match the dataType");
            }
        }

        private bool CheckString(DataField data)
        {
            int a;
            int b;
            var inputString = data.Data.First().ToLower();
            var matchString = DataMatch.First().ToLower();
            DateTime inputYear;
            DateTime matchYear;

            switch (Rule)
            {
                case CriteriaType.Equals:
                    return DataMatch.SequenceEqual(data.Data);
                case CriteriaType.LargerThan:
                    int.TryParse(inputString, out a);
                    int.TryParse(matchString, out b);
                    return a > b;
                case CriteriaType.SmallerThan:
                    int.TryParse(inputString, out a);
                    int.TryParse(matchString, out b);
                    return a < b;
                case CriteriaType.Exists:
                    return data.Data.Any();
                case CriteriaType.Contains:
                    return inputString.Contains(matchString);
                case CriteriaType.BeforeYear:
                    inputYear = Convert.ToDateTime(inputString);
                    matchYear = Convert.ToDateTime(matchString);
                    return inputYear.Year < matchYear.Year;
                case CriteriaType.AfterYear:
                    inputYear = Convert.ToDateTime(inputString);
                    matchYear = Convert.ToDateTime(matchString);
                    return inputYear.Year > matchYear.Year;
                case CriteriaType.IsYear:
                    inputYear = Convert.ToDateTime(inputString);
                    matchYear = Convert.ToDateTime(matchString);
                    return inputYear.Year == matchYear.Year;
                default:
                    throw new ArgumentOutOfRangeException("The criteria rule doesn't match the dataType");
            }
        }

    }


}
