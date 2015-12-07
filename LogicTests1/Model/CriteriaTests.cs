﻿using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.CriteriaValidator;

namespace LogicTests1.Model
{
    [TestClass]
    public class CriteriaTests
    {
        CriteriaValidator _criteriaValidator = new CriteriaValidator();
        Criteria testCriteria1;
        Criteria testCriteria2;
        Criteria testCriteria3;
        Criteria testCriteria4;
        Criteria testCriteria5;
        DataField testDataField;

        [TestInitialize]
        public void setup()
        {
            testCriteria1 = new Criteria()
            {
                Name = "testCriteria",
                Description = "this is a test Criteria"
            };
            testCriteria2 = new Criteria()
            {
                Name = "testCriteria2",
                Description = "this is a test Criteria"
            };
        }
      

        [TestMethod]
        public void CriteriaCheckResourceExistsTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataField.DataType.Resource;

            var data = new string[1];
            data[0] = "data";

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Resource,
                Data = data
            };
            
            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckFlagsContainsExcactTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataField.DataType.Flags;

            string[] expectedData = new string[3]
            {
               "1", "2", "3"
            };

            testCriteria1.DataMatch = expectedData;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = expectedData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CrietriaCheckFlagsContainsMoreTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataField.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[4]
           {
               "1", "2", "3","4"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckFlagsContainsBackwardsTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataField.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[4]
           {
             "4", "3", "2",  "1"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckFlagsContainsFalseTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataField.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[2]
           {
               "1", "2"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckFlagsContainsTrueTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataField.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[3]
           {
               "1", "2", "3"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckFlagsEqualsTrueRandomOrderTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataField.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[3]
           {
               "2","3","1"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckFlagsEqualsFalseTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataField.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[1]
           {
               "1"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckFlagsLargerThanTrueSmallerThanFalseTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.LargerThan;
            testCriteria1.DataType = DataField.DataType.Flags;
            testCriteria2.Rule = Criteria.CriteriaRule.SmallerThan;
            testCriteria2.DataType = DataField.DataType.Flags;

            string[] checkData = new string[1]
            {
               "1"
            };

            string[] actaulData = new string[3]
           {
               "1", "2", "3"
           };

            testCriteria1.DataMatch = checkData;
            testCriteria2.DataMatch = checkData;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria2, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckFlagsLargerThanFalseSmallerThanTrueTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.LargerThan;
            testCriteria1.DataType = DataField.DataType.Flags;
            testCriteria2.Rule = Criteria.CriteriaRule.SmallerThan;
            testCriteria2.DataType = DataField.DataType.Flags;

            string[] checkData = new string[3]
            {
              "1", "2", "3"
            };

            string[] actaulData = new string[1]
           {
               "1"
           };

            testCriteria1.DataMatch = checkData;
            testCriteria2.DataMatch = checkData;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria2, testDataField));
        }


        [TestMethod]
        public void CriteriaCheckFlagsExistsTrueTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataField.DataType.Flags;
      
       
            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = new string[1]
           {
               "1"
           }
        };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckFlagsExistsFalseTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataField.DataType.Flags;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = null
            };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckFlagsExistsEmptyStringTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataField.DataType.Flags;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Flags,
                Data = new string[1] {""}
            };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }


        [TestMethod]
        public void CriteraCheckEnumEqualsTrueTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataField.DataType.Enumeration;
            
            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Enumeration,
                Data = new string[1] {"1"}
            };

            testCriteria1.DataMatch = new string[1] {"1"};

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckEnumEqualsFalseTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataField.DataType.Enumeration;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Enumeration,
                Data = new string[1] { "2" }
            };

            testCriteria1.DataMatch = new string[1] { "1" };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckBoolEqualsTrueTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataField.DataType.Boolean;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Boolean,
                Data = new string[1] { "true" }
            };

            testCriteria1.DataMatch = new string[1] { "true" };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckBoolEqualsFalseTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataField.DataType.Boolean;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Boolean,
                Data = new string[1] { "true" }
            };

            testCriteria1.DataMatch = new string[1] { "false" };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteraCheckStringContainsTrueTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataField.DataType.String;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.String,
                Data = new string[1] { "testing" }
            };

            testCriteria1.DataMatch = new string[1] { "testing" };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaCheckStringContainsTrueMoreWordsTest()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataField.DataType.String;

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.String,
                Data = new string[1] { "testing the test" }
            };

            testCriteria1.DataMatch = new string[1] { "testing" };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CriteriaValidatorCallsCheckerCorrectlyTest()
        {
            //Arrange
            var criteria = new Criteria();
            var data = new DataField();
            var mockCriteriaChecker = new Mock<ICriteriaChecker>();
            var validator = new CriteriaValidator(
                new Dictionary<DataField.DataType, ICriteriaChecker>
                {
                    {It.IsAny<DataField.DataType>(), mockCriteriaChecker.Object}
                });

            mockCriteriaChecker.Setup(m => m.Validate(criteria, data)).Returns(true);

            //Action
            var result = validator.CriteriaIsMet(criteria, data);

            //Assert
            mockCriteriaChecker.Verify(m => m.Validate(criteria, data), Times.Once);
            Assert.IsTrue(result);
        }

    }

}
