#region Using

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyExecution.TaskManagement.CriteriaValidation;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyConfiguration.TaskManagement.CriteriaChecker
{
    [TestClass]
    public class RuleCheckerTests
    {
        private readonly CriteriaValidator _criteriaValidator = new CriteriaValidator();
        private Criteria _testCriteria1;
        private Criteria _testCriteria2;


        [TestInitialize]
        public void Setup()
        {
            _testCriteria1 = new Criteria
            {
                Name = "testCriteria",
                Description = "this is a test Criteria"
            };
            _testCriteria2 = new Criteria
            {
                Name = "testCriteria2",
                Description = "this is a test Criteria"
            };
        }


        [TestMethod]
        public void TestCriteriaCheckResourceExists()
        {
            //Arrange
            _testCriteria1.DataMatch = new List<StoredString>();
            _testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            _testCriteria1.DataType = DataField.DataType.Resource;

            var data = new string[1];
            data[0] = "data";

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, data));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsContainsExcact()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            _testCriteria1.DataType = DataField.DataType.Flags;


            var expectedData = new string[3]
            {
                "1", "2", "3"
            };

            _testCriteria1.DataMatch = expectedData.Select(s => new StoredString {Value = s}).ToList();

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, expectedData));
        }

        [TestMethod]
        public void TestCritriaCheckFlagsContainsMore()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            _testCriteria1.DataType = DataField.DataType.Flags;

            var checkData = new string[3]
            {
                "1", "2", "3"
            };

            var actaulData = new string[4]
            {
                "1", "2", "3", "4"
            };

            _testCriteria1.DataMatch = checkData.Select(s => new StoredString {Value = s}).ToList();

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsContainsBackwards()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            _testCriteria1.DataType = DataField.DataType.Flags;

            var checkData = new string[3]
            {
                "1", "2", "3"
            };

            var actaulData = new string[4]
            {
                "4", "3", "2", "1"
            };

            _testCriteria1.DataMatch = checkData.Select(s => new StoredString {Value = s}).ToList();


            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsContainsFalse()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            _testCriteria1.DataType = DataField.DataType.Flags;

            var checkData = new string[3]
            {
                "1", "2", "3"
            };

            var actaulData = new string[2]
            {
                "1", "2"
            };

            _testCriteria1.DataMatch = checkData.Select(s => new StoredString {Value = s}).ToList();


            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsContainsTrue()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            _testCriteria1.DataType = DataField.DataType.Flags;

            var checkData = new string[3]
            {
                "1", "2", "3"
            };

            var actaulData = new string[3]
            {
                "1", "2", "3"
            };

            _testCriteria1.DataMatch = checkData.Select(s => new StoredString {Value = s}).ToList();


            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsEqualsTrueRandomOrder()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            _testCriteria1.DataType = DataField.DataType.Flags;

            var checkData = new string[3]
            {
                "1", "2", "3"
            };

            var actaulData = new string[3]
            {
                "2", "3", "1"
            };

            _testCriteria1.DataMatch = checkData.Select(s => new StoredString {Value = s}).ToList();


            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsEqualsFalse()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            _testCriteria1.DataType = DataField.DataType.Flags;

            var checkData = new string[3]
            {
                "1", "2", "3"
            };

            var actaulData = new string[1]
            {
                "1"
            };

            _testCriteria1.DataMatch = checkData.Select(s => new StoredString {Value = s}).ToList();


            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsLargerThanTrueSmallerThanFalse()
        {
            //Arrange
            _testCriteria1.DataMatch = new List<StoredString>();
            _testCriteria1.Rule = Criteria.CriteriaRule.LargerThan;
            _testCriteria1.DataType = DataField.DataType.Flags;
            _testCriteria2.DataMatch = new List<StoredString>();
            _testCriteria2.Rule = Criteria.CriteriaRule.SmallerThan;
            _testCriteria2.DataType = DataField.DataType.Flags;


            var checkData = new string[1]
            {
                "1"
            };

            var actaulData = new string[3]
            {
                "1", "2", "3"
            };

            _testCriteria1.DataMatch = checkData.Select(s => new StoredString {Value = s}).ToList();
            _testCriteria1.DataMatch = checkData.Select(s => new StoredString {Value = s}).ToList();

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(_testCriteria2, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsLargerThanFalseSmallerThanTrue()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.LargerThan;
            _testCriteria1.DataType = DataField.DataType.Flags;
            _testCriteria2.Rule = Criteria.CriteriaRule.SmallerThan;
            _testCriteria2.DataType = DataField.DataType.Flags;

            var checkData = new string[3]
            {
                "1", "2", "3"
            };

            var actaulData = new string[1]
            {
                "1"
            };

            _testCriteria1.DataMatch = checkData.Select(s => new StoredString {Value = s}).ToList();
            _testCriteria1.DataMatch = checkData.Select(s => new StoredString {Value = s}).ToList();

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria2, actaulData));
        }


        [TestMethod]
        public void TestCriteriaCheckFlagsExistsTrue()
        {
            //Arrange
            _testCriteria1.DataMatch = new List<StoredString>();
            _testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            _testCriteria1.DataType = DataField.DataType.Flags;


            var actaulData = new string[1]
            {
                "1"
            };


            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsExistsFalse()
        {
            //Arrange
            _testCriteria1.DataMatch = new List<StoredString>();
            _testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            _testCriteria1.DataType = DataField.DataType.Flags;

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(_testCriteria1, null));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsExistsEmptyString()
        {
            //Arrange
            _testCriteria1.DataMatch = new List<StoredString>();
            _testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            _testCriteria1.DataType = DataField.DataType.Flags;


            var actaulData = new string[1] {""};


            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }


        [TestMethod]
        public void TestCriteraCheckEnumEqualsTrue()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            _testCriteria1.DataType = DataField.DataType.Enumeration;


            var actaulData = new string[1] {"1"};


            _testCriteria1.DataMatch = new List<StoredString> {new StoredString {Value = "1"}};


            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckEnumEqualsFalse()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            _testCriteria1.DataType = DataField.DataType.Enumeration;


            var actaulData = new string[1] {"2"};


            _testCriteria1.DataMatch = new List<StoredString> {new StoredString {Value = "1"}};

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckBoolEqualsTrue()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            _testCriteria1.DataType = DataField.DataType.Boolean;


            var actaulData = new string[1] {"true"};
            _testCriteria1.DataMatch = new List<StoredString> {new StoredString {Value = "true"}};

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckBoolEqualsFalse()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            _testCriteria1.DataType = DataField.DataType.Boolean;


            var actaulData = new string[1] {"true"};

            _testCriteria1.DataMatch = new List<StoredString> {new StoredString {Value = "1"}};

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteraCheckStringContainsTrue()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            _testCriteria1.DataType = DataField.DataType.String;


            var actaulData = new string[1] {"testing"};

            _testCriteria1.DataMatch = new List<StoredString> {new StoredString {Value = "testing"}};

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteraStringLargerThan()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.LargerThan;
            _testCriteria1.DataType = DataField.DataType.String;


            var actaulData = new string[1] {"2014"};

            _testCriteria1.DataMatch = new List<StoredString> {new StoredString {Value = "2013"}};

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }


        [TestMethod]
        public void TestCriteriaCheckStringContainsTrueMoreWords()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            _testCriteria1.DataType = DataField.DataType.String;


            var actaulData = new string[1] {"testing the test"};


            _testCriteria1.DataMatch = new List<StoredString> {new StoredString {Value = "testing"}};

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaBeforeDate()
        {
            //Arrange
            _testCriteria1.Rule = Criteria.CriteriaRule.BeforeDate;
            _testCriteria1.DataType = DataField.DataType.String;


            var actaulData = new string[1] {"1/12/1800"};
            _testCriteria1.DataMatch = new List<StoredString> {new StoredString {Value = "2/3/1999"}};

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(_testCriteria1, actaulData));
        }
    }
}