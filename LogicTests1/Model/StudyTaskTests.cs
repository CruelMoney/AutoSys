﻿using System;
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
    public class StudyTaskTests
    {
        StudyTask testTask;
        StudyTask testTaskMultipleUsers;


        [TestInitialize]
        public void Initialize()
        {

            var testItem = new Item(Item.ItemType.Book, new Dictionary<Item.FieldType, string>());
            var testUser1 = new User() { Id = 1, Name = "chris" };
            var testUser2 = new User() { Id = 2, Name = "ramos" };
            var userData1 = new UserData() { User = testUser1, Data = new string[1] { "initialData" } };
            var userData2 = new UserData() { User = testUser2, Data = new string[1] { "initialData2" } };
            var dataFields1 = new List<DataField>() { new DataField() { UserData = {userData1} , Name = "testField", Description = "testDescription" } };
            var dataFields2 = new List<DataField>() { new DataField() { UserData = { userData2, userData1 }, Name = "testField2", Description = "testDescription2" } };

            testTask = new StudyTask()
            {
                Paper = testItem,
                DataFields = new List<DataField>(dataFields1)
            };

            testTaskMultipleUsers = new StudyTask()
            {
                Paper = testItem,
                DataFields = new List<DataField>(dataFields2)
            };
        }

        [TestMethod]
        public void SubmitDataSingleUser()
        {
            //Arrange
            var expectedData = new string[] {"testData"};
            var dataFields = new DataFieldDTO[1]  { new DataFieldDTO(){ Data = expectedData, Name = "testField" } };
            var dataSubmit = new TaskSubmissionDTO()
            {
                SubmittedFieldsDto = dataFields, UserId = 1
            };

            //Action
            var actualTask = testTask.SubmitData(dataSubmit);

            //Assert
            var actualData = actualTask.DataFields.First(d => d.Name.Equals("testField"));
            var actualUserData = actualData.UserData.First(d => d.User.Name.Equals("chris"));
      
            Assert.AreEqual(expectedData, actualUserData.Data);
        }

        [TestMethod]
        public void SubmitDataMultipleUsers()
        {
            //Arrange
            var expectedData = new[] { "testData" };
          
            var dataFields = new DataFieldDTO[1] { new DataFieldDTO() { Data = expectedData, Name = "testField2" } };
            var dataSubmit = new TaskSubmissionDTO()
            {
                SubmittedFieldsDto = dataFields,
                UserId = 2
            };

            //Action
            var actualTask = testTaskMultipleUsers.SubmitData(dataSubmit);

            //Assert
            var actualData = actualTask.DataFields.First(d => d.Name.Equals("testField2"));
            var actualUserData1 = actualData.UserData.First(d => d.User.Id.Equals(1));
            var actualUserData2 = actualData.UserData.First(d => d.User.Id.Equals(2));

            Assert.AreEqual("initialData", actualUserData1.Data[0]);
            Assert.AreEqual("testDescription2", actualData.Description);
            Assert.AreEqual(expectedData, actualUserData2.Data);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SubmitDataInvalidUser()
        {
            //Arrange
            var expectedData = new string[] { "testData" };
            var dataFields = new DataFieldDTO[1] { new DataFieldDTO() { Data = expectedData, Name = "testField" } };
            var dataSubmit = new TaskSubmissionDTO()
            {
                SubmittedFieldsDto = dataFields,
                UserId = 2
            };

            //Action
            testTask.SubmitData(dataSubmit);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubmitDataInvalidDatafield()
        {
            //Arrange
            var expectedData = new string[] { "testData" };
            var dataFields = new DataFieldDTO[1] { new DataFieldDTO() { Data = expectedData, Name = "invalidTestField" } };
            var dataSubmit = new TaskSubmissionDTO()
            {
                SubmittedFieldsDto = dataFields,
                UserId = 1
            };

            //Action
            testTask.SubmitData(dataSubmit);
        }
    }
}
