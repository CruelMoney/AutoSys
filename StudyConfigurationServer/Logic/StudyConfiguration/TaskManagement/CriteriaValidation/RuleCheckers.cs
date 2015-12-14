using System;
using System.Collections.Generic;
using System.Linq;

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.CriteriaValidation
{

    public class DefaultRuleChecker : IRuleChecker
    {
        public bool IsRuleMet(ICollection<string> data, ICollection<string> criteriaData)
        {
            throw new NotImplementedException();
        }
    }


    //TODO Maybe check if the source is avaliable if the datatype is resource. Don't know excactly how sources work.

    /// <summary>
    ///     Checks if the data array contains any data. Empty Strings will return true.
    /// </summary>
    /// <returns></returns>
    public class DataExistsRule : IRuleChecker
    {
        public bool IsRuleMet(ICollection<string> data, ICollection<string> criteriaData = null)
        {
            return !(data == null || !data.Any());
        }
    }

    /// <summary>
    ///     Checks if the given set is a subset. Ignoring order.
    ///     If the data only contains 1 object the strings are compared to see if the given string is contained in the other.
    /// </summary>
    /// <param name="list1">The data that is checked if it contains</param>
    /// <param name="list2">The data that is checked if is contained</param>
    /// <returns></returns>
    public class DataContainsRule : IRuleChecker
    {
        public bool IsRuleMet(ICollection<string> data, ICollection<string> criteriaData)
        {
            if (data.Count() != 1 && criteriaData.Count() != 1)
            {
                return !criteriaData.Except(data).Any();
            }

            return data.First().Contains(criteriaData.First());
        }
    }


    /// <summary>
    ///     Checks if the two sets are equal ignoring order.
    /// </summary>
    /// <param name="list1">First set</param>
    /// <param name="list2">Second set</param>
    /// <returns></returns>
    public class EqualIgnoreOrderRule : IRuleChecker

    {
        public bool IsRuleMet(ICollection<string> data, ICollection<string> criteriaData)
        {
            return data.OrderBy(t => t).SequenceEqual(criteriaData.OrderBy(t => t));
        }
    }

    /// <summary>
    ///     Checks if set is larger than the given set.
    ///     If the set contains only one object, the first objects are compared as numbers.
    /// </summary>
    /// <param name="set1"></param>
    /// <param name="set2"></param>
    /// <returns></returns>
    public class LargerThanRule : IRuleChecker
    {
        public bool IsRuleMet(ICollection<string> data, ICollection<string> criteriaData)
        {
            if (data.Count() != 1 || criteriaData.Count() != 1)
            {
                return data.Count() > criteriaData.Count();
            }

            try
            {
                var first = decimal.Parse(data.First());
                var second = decimal.Parse(criteriaData.First());
                return first > second;
            }
            catch (Exception)
            {
                throw new ArgumentException("The data does not contain valid string for converting to decimal");
            }
        }
    }


    /// <summary>
    ///     Checks if the set is smaller than the given.
    ///     If the set contains only one object, the first objects are compared as numbers.
    /// </summary>
    /// <param name="set1"></param>
    /// <param name="set2"></param>
    /// <returns></returns>
    public class SmallerThanRule : IRuleChecker
    {
        public bool IsRuleMet(ICollection<string> data, ICollection<string> criteriaData)
        {
            if (data.Count() != 1 || criteriaData.Count() != 1)
            {
                return data.Count() < criteriaData.Count();
            }

            try
            {
                var first = decimal.Parse(data.First());
                var second = decimal.Parse(criteriaData.First());
                return first < second;
            }
            catch (Exception)
            {
                throw new ArgumentException("The data does not contain valid string for converting to decimal");
            }
        }
    }


    /// <summary>
    ///     Checks if the given year comes after the year.
    /// </summary>
    /// <param name="year1"></param>
    /// <param name="year2"></param>
    /// <returns></returns>
    public class BeforeYearRule : IRuleChecker
    {
        public bool IsRuleMet(ICollection<string> data, ICollection<string> criteriaData)
        {
            if (data.Count() > 1 || criteriaData.Count() > 1)
            {
                throw new ArgumentOutOfRangeException("The data array contains more than 1 object");
            }
            try
            {
                DateTime checkYear;
                DateTime.TryParse(data.First(), out checkYear);
                var inputYear = Convert.ToDateTime(criteriaData.First());
                return inputYear.Year > checkYear.Year;
            }
            catch (Exception)
            {
                throw new ArgumentException("The data does not contain valid strings for converting to datetime");
            }
        }
    }


    /// <summary>
    ///     Checks if the given year comes before the year.
    /// </summary>
    /// <param name="year1"></param>
    /// <param name="year2"></param>
    /// <returns></returns>
    public class AfterYearRule : IRuleChecker
    {
        public bool IsRuleMet(ICollection<string> data, ICollection<string> criteriaData)
        {
            if (data.Count() > 1 || criteriaData.Count() > 1)
            {
                throw new ArgumentOutOfRangeException("The data array contains more than 1 object");
            }
            try
            {
                var checkYear = Convert.ToDateTime(data.First());
                var inputYear = Convert.ToDateTime(criteriaData.First());
                return inputYear.Year < checkYear.Year;
            }
            catch (Exception)
            {
                throw new ArgumentException("The data does not contain valid strings for converting to datetime");
            }
        }
    }

    public class IsYear : IRuleChecker
    {
        public bool IsRuleMet(ICollection<string> data, ICollection<string> criteriaData)
        {
            if (data.Count() > 1 || criteriaData.Count() > 1)
            {
                throw new ArgumentOutOfRangeException("The data array contains more than 1 object");
            }
            try
            {
                var checkYear = Convert.ToDateTime(data.First());
                var inputYear = Convert.ToDateTime(criteriaData.First());
                return inputYear.Year.Equals(checkYear.Year);
            }
            catch (Exception)
            {
                throw new ArgumentException("The data does not contain valid strings for converting to datetime");
            }
        }
    }
}
