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

            
        }
      

        [TestMethod]
        public void CheckResourceExists()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaType.Exists;
            testCriteria1.DataType = DataField.DataType.Resource;

            var data = new string[1];
            data[0] = "data";

            testDataField = new DataField()
            {
                FieldType = DataField.DataType.Resource,
                Data = data
            };
            
            //Assert
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
        }

        [TestMethod]
        public void CheckFlags_Contains_Excact()
        {
            //Arrange
            testCriteria1.Rule = Criteria.CriteriaType.Contains;
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
            Assert.IsTrue(testCriteria1.CriteriaIsMet(testDataField));
        }
    }
}
