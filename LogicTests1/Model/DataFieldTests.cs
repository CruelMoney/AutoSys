using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace LogicTests1.Model
{
    [TestClass]
    public class DataFieldTests
    {
        UserData userData1;
        UserData userData2;


        [TestInitialize]
        public void Initialize()
        {

            var testUser1 = new User() { Id = 1, Name = "chris" };
            var testUser2 = new User() { Id = 2, Name = "ramos" };
            userData1 = new UserData() { User = testUser1, Data = new string[1] { "initialData" } };
            userData2 = new UserData() { User = testUser2, Data = new string[1] { "initialData2" } };
            var dataField1 =  new DataField() { UserData = { userData1 }, Name = "testField", Description = "testDescription"  };
            var dataField2 =  new DataField() { UserData = { userData2, userData1 }, Name = "testField2", Description = "testDescription2"  };
          
        }

        [TestMethod]
        public void SubmitDataSingleUser()
        {
            //Arrange
            var expectedData = new string[] {"testData"};
            var dataField = new DataField() { UserData = { userData1 }, Name = "testField", Description = "testDescription" };

            //Action
            var actualField = dataField.SubmitData(1, expectedData);
            var actualUserData = actualField.UserData.First(u => u.User.Id == 1);

            //Assert
           
            Assert.AreEqual("chris", actualUserData.User.Name);
            Assert.AreEqual(expectedData, actualUserData.Data);
            Assert.AreEqual("testDescription", actualField.Description);
        }

        [TestMethod]
        public void SubmitDataMultipleUsers()
        {
            //Arrange
            var expectedData = new string[] { "testData" };
            var dataField = new DataField() { UserData = { userData1, userData2 }, Name = "testField", Description = "testDescription" };
            
            //Action
            var actualField2 = dataField.SubmitData(2, expectedData);

            //Assert
            var actualField1 = dataField.SubmitData(1, expectedData);
            var actualUserData1 = actualField1.UserData.First(u => u.User.Id == 1);
            var actualUserData2 = actualField2.UserData.First(u => u.User.Id == 2);

            Assert.AreEqual("ramos", actualUserData2.User.Name);
            Assert.AreEqual(expectedData, actualUserData2.Data);
            Assert.AreEqual("testDescription", actualField2.Description);

            Assert.AreEqual("initialData", actualUserData1.Data[0]);
            Assert.AreEqual("testDescription", actualField1.Description);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SubmitDataInvalidUser()
        {
            //Arrange
            var expectedData = new string[] { "testData" };
            var dataField = new DataField() { UserData = { userData1 }, Name = "testField", Description = "testDescription" };

            //Action
            var actualField = dataField.SubmitData(2, expectedData);
            
        }

       
    }
}
