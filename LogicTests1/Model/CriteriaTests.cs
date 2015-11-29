using System;
using Logic.Model;
using Logic.Model.CriteriaValidator;
using Logic.Model.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

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
        DataFieldLogic testDataField;

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
        public void CheckResourceExists()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataFieldLogic.DataType.Resource;

            var data = new string[1];
            data[0] = "data";

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Resource,
                Data = data
            };
            
            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckFlags_Contains_Excact()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            string[] expectedData = new string[3]
            {
               "1", "2", "3"
            };

            testCriteria1.DataMatch = expectedData;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = expectedData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckFlags_Contains_More()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[4]
           {
               "1", "2", "3","4"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckFlags_Contains_Backwards()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[4]
           {
             "4", "3", "2",  "1"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckFlags_Contains_False()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[2]
           {
               "1", "2"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckFlags_Equals_true()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[3]
           {
               "1", "2", "3"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckFlags_Equals_true_random_order()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[3]
           {
               "2","3","1"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckFlags_Equals_false()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[1]
           {
               "1"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckFlags_larger_than_true_smallerThan_false()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.LargerThan;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;
            testCriteria2.Rule = Criteria.CriteriaRule.SmallerThan;
            testCriteria2.DataType = DataFieldLogic.DataType.Flags;

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

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria2, testDataField));
        }

        [TestMethod]
        public void CheckFlags_larger_than_false_smallerThan_true()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.LargerThan;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;
            testCriteria2.Rule = Criteria.CriteriaRule.SmallerThan;
            testCriteria2.DataType = DataFieldLogic.DataType.Flags;

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

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria2, testDataField));
        }


        [TestMethod]
        public void CheckFlags_exists_true()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;
      
       
            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = new string[1]
           {
               "1"
           }
        };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckFlags_exists_null()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = null
            };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckFlags_exists_empty_string()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = new string[1] {""}
            };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }


        [TestMethod]
        public void CheckEnum_equals_true()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Exists;
            testCriteria1.DataType = DataFieldLogic.DataType.Enumeration;
            
            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Enumeration,
                Data = new string[1] {"1"}
            };

            testCriteria1.DataMatch = new string[1] {"1"};

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckEnum_equals_false()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataFieldLogic.DataType.Enumeration;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Enumeration,
                Data = new string[1] { "2" }
            };

            testCriteria1.DataMatch = new string[1] { "1" };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckBool_equals_true()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataFieldLogic.DataType.Boolean;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Boolean,
                Data = new string[1] { "true" }
            };

            testCriteria1.DataMatch = new string[1] { "true" };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckBool_equals_false()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Equals;
            testCriteria1.DataType = DataFieldLogic.DataType.Boolean;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Boolean,
                Data = new string[1] { "true" }
            };

            testCriteria1.DataMatch = new string[1] { "false" };

            //Assert
            Assert.IsFalse(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckString_contains_true()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataFieldLogic.DataType.String;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.String,
                Data = new string[1] { "testing" }
            };

            testCriteria1.DataMatch = new string[1] { "testing" };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

        [TestMethod]
        public void CheckString_contains_true_more_words()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaRule.Contains;
            testCriteria1.DataType = DataFieldLogic.DataType.String;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.String,
                Data = new string[1] { "testing the test" }
            };

            testCriteria1.DataMatch = new string[1] { "testing" };

            //Assert
            Assert.IsTrue(_criteriaValidator.CriteriaIsMet(testCriteria1, testDataField));
        }

    }

}
