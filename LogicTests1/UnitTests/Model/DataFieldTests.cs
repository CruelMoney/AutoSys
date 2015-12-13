using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        DataField dataField1;
        DataField dataField2;


        [TestInitialize]
        public void Initialize()
        {

            var testUser1 = new User() { ID = 1, Name = "chris" };
            var testUser2 = new User() { ID = 2, Name = "ramos" };
            userData1 = new UserData() { UserID = 1, Data = new List<StoredString>() { new StoredString() { Value = "initialData" }} };
            userData2 = new UserData() { UserID = 2, Data = new List<StoredString>() { new StoredString() { Value = "initialData2" }} };
            dataField1 =  new DataField() { UserData = new List<UserData>() { userData1 }, Name = "testField", Description = "testDescription"  };
            dataField2 =  new DataField() { UserData = new List<UserData>(){ userData2, userData1 }, Name = "testField2", Description = "testDescription2"  };
          
        }

        [TestMethod]
        public void TestDataFieldSubmitDataSingleUser()
        {
            //Arrange
            var expectedData = new string[] {"testData"};
            var dataField = new DataField() { UserData = new List<UserData>() { userData1 }, Name = "testField", Description = "testDescription" };

            //Action
            var actualField = dataField.SubmitData(1, expectedData);
            var actualUserData = actualField.UserData.First(u => u.UserID == 1);

            //Assert
           
            Assert.AreEqual(1, actualUserData.UserID);
            Assert.AreEqual(expectedData[0], actualUserData.Data.First().Value);
            Assert.AreEqual("testDescription", actualField.Description);
        }

        [TestMethod]
        public void TestDataFieldSubmitDataMultipleUsers()
        {
            //Arrange
            var expectedData = new string[] { "testData" };
            var dataField = new DataField() { UserData = new List<UserData>() { userData1, userData2 }, Name = "testField", Description = "testDescription" };
            
            //Action
            dataField.SubmitData(2, expectedData);

            //Assert
            var actualUserData1 = dataField.UserData.First(u => u.UserID == 1);
            var actualUserData2 = dataField.UserData.First(u => u.UserID == 2);

            Assert.AreEqual(2, actualUserData2.UserID);
            Assert.AreEqual(expectedData[0], actualUserData2.Data.First().Value);
            Assert.AreEqual("testDescription", dataField.Description);

            Assert.AreEqual("initialData", actualUserData1.Data[0].Value);
            Assert.AreEqual("testDescription", dataField.Description);
        }


        [TestMethod]
        [ExpectedException(typeof(TargetException))]
        public void TestDataFieldSubmitDataInvalidUser()
        {
            //Arrange
            var expectedData = new string[] { "testData" };
            var dataField = new DataField() { UserData = new List<UserData>() { userData1 }, Name = "testField", Description = "testDescription" };

            //Action
            var actualField = dataField.SubmitData(2, expectedData);
            
        }

        [TestMethod]
        public void TestDataFieldSubmitDataMultipleData()
        {
            //Arrange
            var expectedData = new string[] { "testData", "testData2", "testData3" };
            var dataField = new DataField() { UserData = new List<UserData>() { userData1, userData2 }, Name = "testField", Description = "testDescription" };

            //Action
            dataField.SubmitData(2, expectedData);

            //Assert
            var actualUserData1 = dataField.UserData.First(u => u.UserID == 1);
            var actualUserData2 = dataField.UserData.First(u => u.UserID == 2);

            Assert.AreEqual(2, actualUserData2.UserID);
            Assert.AreEqual(expectedData[0], actualUserData2.Data[0].Value);
            Assert.AreEqual(expectedData[1], actualUserData2.Data[1].Value);
            Assert.AreEqual(expectedData[2], actualUserData2.Data[2].Value);
            Assert.AreEqual("testDescription", dataField.Description);

            Assert.AreEqual("initialData", actualUserData1.Data[0].Value);
            Assert.AreEqual("testDescription", dataField.Description);

            
        }

        [TestMethod]
        public void UserDataIsConflicting()
        {
            //Arrange
            var userData3 = new UserData() { UserID = 1, Data = new List<StoredString>() { new StoredString() { Value = "true" } } };
            var userData4 = new UserData() { UserID = 2, Data = new List<StoredString>() { new StoredString() { Value = "false" } } };
            var userData5 = new UserData() { UserID = 2, Data = new List<StoredString>() { new StoredString() { Value = "true" } } };


            var dataField = new DataField()
            {
                UserData = new List<UserData>() { userData3, userData4 }
            };

            //Action
            var result = dataField.UserDataIsConflicting();

            //Assert
            Assert.IsTrue(result);

        }


        [TestMethod]
        public void UserDataIsNotConflicting()
        {
            //Arrange
            var userData3 = new UserData() { UserID = 1, Data = new List<StoredString>() { new StoredString() { Value = "initialData" } } };
            var userData4 = new UserData() { UserID = 2, Data = new List<StoredString>() { new StoredString() { Value = "initialData" } } };
            var userData5 = new UserData() { UserID = 2, Data = new List<StoredString>() { new StoredString() { Value = "initialData" } } };


            var dataField = new DataField()
            {
                UserData = new List<UserData>() { userData3, userData4, userData5 }
            };

            //Action
            var result = dataField.UserDataIsConflicting();

            //Assert
            Assert.IsFalse(result);

        }

        [TestMethod]
        public void MultipleUserDataIsNotConflicting()
        {
            //Arrange
            var userData3 = new UserData() { UserID = 1, Data = new List<StoredString>() { new StoredString() { Value = "initialData" }, new StoredString() { Value = "initialData2" } } };
            var userData4 = new UserData() { UserID = 2, Data = new List<StoredString>() { new StoredString() { Value = "initialData" }, new StoredString() { Value = "initialData2" } } };

            var dataField = new DataField()
            {
                UserData = new List<UserData>() { userData3, userData4 }
            };

            //Action
            var result = dataField.UserDataIsConflicting();

            //Assert
            Assert.IsFalse(result);

        }


        [TestMethod]
        public void MultipleUserDataIsConflicting()
        {
            //Arrange
            var userData3 = new UserData() { UserID = 1, Data = new List<StoredString>() { new StoredString() { Value = "initialData" }, new StoredString() { Value = "initialData" } } };
            var userData4 = new UserData() { UserID = 2, Data = new List<StoredString>() { new StoredString() { Value = "initialData2" }, new StoredString() { Value = "initialData" } } };

            var dataField = new DataField()
            {
                UserData = new List<UserData>() { userData3, userData4 }
            };

            //Action
            var result = dataField.UserDataIsConflicting();

            //Assert
            Assert.IsTrue(result);

        }

    }
}
