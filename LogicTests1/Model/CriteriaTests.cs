using System;
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
        public void TestCriteriaCheckResourceExists()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataField.DataType.Resource;

            var data = new string[1];
            data[0] = "data";
        
            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, data));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsContainsExcact()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataField.DataType.Flags;
            

            string[] expectedData = new string[3]
            {
               "1", "2", "3"
            };

            testCriteria1.DataMatch = expectedData;

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, expectedData));
        }

        [TestMethod]
        public void TestCritriaCheckFlagsContainsMore()
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

          
            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsContainsBackwards()
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

          

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsContainsFalse()
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

           

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsContainsTrue()
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


            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsEqualsTrueRandomOrder()
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

          

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsEqualsFalse()
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


            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsLargerThanTrueSmallerThanFalse()
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

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria2, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsLargerThanFalseSmallerThanTrue()
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

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria2, actaulData));
        }


        [TestMethod]
        public void TestCriteriaCheckFlagsExistsTrue()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataField.DataType.Flags;



            var actaulData = new string[1]
            {
                "1"
            };
        

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsExistsFalse()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataField.DataType.Flags;

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, null));
        }

        [TestMethod]
        public void TestCriteriaCheckFlagsExistsEmptyString()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataField.DataType.Flags;


            var actaulData = new string[1] {""};
            

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }


        [TestMethod]
        public void TestCriteraCheckEnumEqualsTrue()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataField.DataType.Enumeration;


            var actaulData = new string[1] {"1"};
            

            testCriteria1.DataMatch = new string[1] {"1"};

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckEnumEqualsFalse()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataField.DataType.Enumeration;


            var actaulData = new string[1] {"2"};
           
        
            testCriteria1.DataMatch = new string[1] { "1" };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckBoolEqualsTrue()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataField.DataType.Boolean;

            
             var actaulData = new string[1] { "true" };
            testCriteria1.DataMatch = new string[1] { "true" };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckBoolEqualsFalse()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataField.DataType.Boolean;


               var actaulData = new string[1] { "true" };

            testCriteria1.DataMatch = new string[1] { "false" };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteraCheckStringContainsTrue()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataField.DataType.String;


            var actaulData = new string[1] {"testing"};
            
            testCriteria1.DataMatch = new string[1] { "testing" };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaCheckStringContainsTrueMoreWords()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataField.DataType.String;


            var actaulData = new string[1] {"testing the test"};
            

            testCriteria1.DataMatch = new string[1] { "testing" };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, actaulData));
        }

        [TestMethod]
        public void TestCriteriaValidatorCallsCheckerCorrectly()
        {
            //Arrange
            var criteria = new Criteria();
            var data = new string[1];
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
