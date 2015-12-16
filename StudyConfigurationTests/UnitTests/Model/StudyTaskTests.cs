#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServerTests.UnitTests.Model
{
    [TestClass]
    public class StudyTaskTests
    {
        private StudyTask _finishedTask;
        private StudyTask _testTask;
        private StudyTask _testTaskFinished;
        private StudyTask _testTaskMultipleUsers;
        private StudyTask _unfinishedTask;


        [TestInitialize]
        public void Initialize()
        {
            var testItem = new Item(Item.ItemType.Book, new Dictionary<FieldType, string>());
            var testUser1 = new User {ID = 1, Name = "chris"};
            var testUser2 = new User {ID = 2, Name = "ramos"};
            var userData1 = new UserData
            {
                UserId = 1,
                Data = new List<StoredString> {new StoredString {Value = "initialData"}}
            };
            var userData2 = new UserData
            {
                UserId = 2,
                Data = new List<StoredString> {new StoredString {Value = "initialData2"}}
            };
            var userData3 = new UserData {UserId = 2, Data = new List<StoredString> {new StoredString()}};
            var dataFields1 = new List<DataField>
            {
                new DataField
                {
                    UserData = new List<UserData> {userData1},
                    Name = "testField",
                    Description = "testDescription"
                }
            };
            var dataFields2 = new List<DataField>
            {
                new DataField
                {
                    UserData = new List<UserData> {userData2, userData1},
                    Name = "testField2",
                    Description = "testDescription2"
                }
            };
            var dataFields3 = new List<DataField>
            {
                new DataField
                {
                    UserData = new List<UserData> {userData3, userData1},
                    Name = "testField3",
                    Description = "testDescription"
                }
            };


            _testTask = new StudyTask
            {
                Paper = testItem,
                DataFields = new List<DataField>(dataFields1)
            };

            _testTaskMultipleUsers = new StudyTask
            {
                Paper = testItem,
                DataFields = new List<DataField>(dataFields2)
            };

            _testTaskFinished = new StudyTask
            {
                Paper = testItem,
                DataFields = new List<DataField>(dataFields3)
            };
        }

        [TestMethod]
        public void TestTaskSubmitDataSingleUser()
        {
            //Arrange
            var expectedData = new[] {"testData"};
            var dataFields = new DataFieldDto[1] {new DataFieldDto {Data = expectedData, Name = "testField"}};
            var dataSubmit = new TaskSubmissionDto
            {
                SubmittedFields = dataFields,
                UserId = 1
            };

            //Action
            var actualTask = _testTask.SubmitData(dataSubmit);

            //Assert
            var actualData = actualTask.DataFields.First(d => d.Name.Equals("testField"));
            var actualUserData = actualData.UserData.First(d => d.UserId.Equals(1));

            Assert.AreEqual(expectedData[0], actualUserData.Data[0].Value);
        }

        [TestMethod]
        public void TestTaskSubmitDataMultipleUsers()
        {
            //Arrange
            var expectedData = new[] {"testData"};

            var dataFields = new DataFieldDto[1] {new DataFieldDto {Data = expectedData, Name = "testField2"}};
            var dataSubmit = new TaskSubmissionDto
            {
                SubmittedFields = dataFields,
                UserId = 2
            };

            //Action
            var actualTask = _testTaskMultipleUsers.SubmitData(dataSubmit);

            //Assert
            var actualData = actualTask.DataFields.First(d => d.Name.Equals("testField2"));
            var actualUserData1 = actualData.UserData.First(d => d.UserId.Equals(1));
            var actualUserData2 = actualData.UserData.First(d => d.UserId.Equals(2));

            Assert.AreEqual("initialData", actualUserData1.Data[0].Value);
            Assert.AreEqual("testDescription2", actualData.Description);
            Assert.AreEqual(expectedData[0], actualUserData2.Data[0].Value);
        }


        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestTaskSubmitDataInvalidUser()
        {
            //Arrange
            var expectedData = new[] {"testData"};
            var dataFields = new DataFieldDto[1] {new DataFieldDto {Data = expectedData, Name = "testField"}};
            var dataSubmit = new TaskSubmissionDto
            {
                SubmittedFields = dataFields,
                UserId = 2
            };

            //Action
            _testTask.SubmitData(dataSubmit);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentException))]
        public void TestTaskSubmitDataInvalidDatafield()
        {
            //Arrange
            var expectedData = new[] {"testData"};
            var dataFields = new DataFieldDto[1] {new DataFieldDto {Data = expectedData, Name = "invalidTestField"}};
            var dataSubmit = new TaskSubmissionDto
            {
                SubmittedFields = dataFields,
                UserId = 1
            };

            //Action
            _testTask.SubmitData(dataSubmit);
        }

        [TestMethod]
        public void TestTaskFinished()
        {
            //Arrange

            //Action
            var allFinish = _testTaskFinished.IsFinished();
            var user1Finish = _testTaskFinished.IsFinished(1);
            var user2Finish = _testTaskFinished.IsFinished(2);

            //Assert
            Assert.IsTrue(user1Finish);
            Assert.IsFalse(user2Finish);
            Assert.IsFalse(allFinish);
        }
    }
}