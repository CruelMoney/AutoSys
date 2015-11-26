using System;
using Logic.Model;
using Logic.Model.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace LogicTests1.Model
{
    [TestClass]
    public class CriteriaTests
    {

        CriteriaLogic testCriteria1;
        CriteriaLogic testCriteria2;
        CriteriaLogic testCriteria3;
        CriteriaLogic testCriteria4;
        CriteriaLogic testCriteria5;
        DataFieldLogic testDataField;

        [TestInitialize]
        public void setup()
        {
            testCriteria1 = new CriteriaLogic()
            {
                Name = "testCriteria",
                Description = "this is a test Criteria"
            };

            
        }
      

        [TestMethod]
        public void CheckResourceExists()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Exists;
            testCriteria1.DataType = DataFieldLogic.DataType.Resource;

            var data = new string[1];
            data[0] = "data";

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Resource,
                Data = data
            };
            
            //Assert
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_Contains_Excact()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Contains;
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
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_Contains_More()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Contains;
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
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_Contains_Backwards()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Contains;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[4]
           {
              "4","3", "2","1"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_Contains_False()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Contains;
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
            Assert.IsFalse(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_Equals_true()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Equals;
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
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_Equals_true_another_order()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Equals;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;

            string[] checkData = new string[3]
            {
               "1", "2", "3"
            };

            string[] actaulData = new string[3]
           {
               "3","2","1"
           };

            testCriteria1.DataMatch = checkData;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = actaulData
            };

            //Assert
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_Equals_false()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Equals;
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
            Assert.IsFalse(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_larger_than_true_smallerThan_false()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.LargerThan;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;
            testCriteria2.Rule = CriteriaLogic.CriteriaType.SmallerThan;
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
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
            Assert.IsFalse(testCriteria2.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_larger_than_false_smallerThan_true()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.LargerThan;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;
            testCriteria2.Rule = CriteriaLogic.CriteriaType.SmallerThan;
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
            Assert.IsFalse(testCriteria1.CriteriaIsMet(testDataField));
            Assert.IsTrue(testCriteria2.CriteriaIsMet(testDataField));
        }


        [TestMethod]
        public void CheckFlags_exists_true()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Exists;
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
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_exists_false()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Exists;
            testCriteria1.DataType = DataFieldLogic.DataType.Flags;


            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Flags,
                Data = new string[1]
          
            };

            //Assert
            Assert.IsFalse(testCriteria1.CriteriaIsMet(testDataField));
        }


        [TestMethod]
        public void CheckEnum_equals_true()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Exists;
            testCriteria1.DataType = DataFieldLogic.DataType.Enumeration;
            
            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Enumeration,
                Data = new string[1] {"1"}
            };

            testCriteria1.DataMatch = new string[1] {"1"};

            //Assert
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckEnum_equals_false()
        {
            //Arrange
            testCriteria1.Rule = CriteriaLogic.CriteriaType.Exists;
            testCriteria1.DataType = DataFieldLogic.DataType.Enumeration;

            testDataField = new DataFieldLogic()
            {
                FieldType = DataFieldLogic.DataType.Enumeration,
                Data = new string[1] { "2" }
            };

            testCriteria1.DataMatch = new string[1] { "1" };

            //Assert
            Assert.IsFalse(testCriteria1.CriteriaIsMet(testDataField));
        }
    }
}
