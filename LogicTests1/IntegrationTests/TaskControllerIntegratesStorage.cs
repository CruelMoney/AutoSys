using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests1.IntegrationTests.DBInitializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.TaskManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;

namespace LogicTests1.IntegrationTests
{
    [TestClass]
    public class TaskControllerIntegratesStorage
    {
        TaskStorageManager _storageManager;
        TaskController _controller;



        //This can reset the database before each test or set it up with custom context
        private void setupEmptyDB()
        {
            Database.SetInitializer(new EmptyDBInitializer());

            var context = new StudyContext();
            context.Database.Initialize(true);
        }

        private void setupMultipleDB()
        {
            Database.SetInitializer(new MultipleStuidesDBInitializer());

            var context = new StudyContext();
            context.Database.Initialize(true);
        }


        [TestInitialize]
        public void Initialize()
        {
             setupMultipleDB();
            _controller = new TaskController();

            var testItem = new Item(Item.ItemType.Book, new Dictionary<Item.FieldType, string>());
            var testUser1 = new User() { Id = 1, Name = "chris" };
            var testUser2 = new User() { Id = 2, Name = "ramos" };
            var userData1 = new UserData() { UserID = 1, Data = new string[1] { "initialData" } };
            var userData2 = new UserData() { UserID = 2, Data = new string[1] { "initialData2" } };
            var userData3 = new UserData() { UserID = 2, Data = new string[1] };
            var dataFields1 = new List<DataField>() { new DataField() { UserData = new List<UserData>() { userData1 }, Name = "testField", Description = "testDescription" } };
            var dataFields2 = new List<DataField>() { new DataField() { UserData = new List<UserData>() { userData2, userData1 }, Name = "testField2", Description = "testDescription2" } };
            var dataFields3 = new List<DataField>() { new DataField() { UserData = new List<UserData>() { userData3, userData1 }, Name = "testField3", Description = "testDescription" } };


            var testTask = new StudyTask()
            {
                Paper = testItem,
                DataFields = new List<DataField>(dataFields2),
                IsEditable = true
            };
            
            _storageManager.CreateTask(testTask);
        }

        [TestMethod]
        public void submitTaskDTO()
        {
            //Arrange
            var expectedData = new string[] { "newData" } ;

            var dto = new TaskSubmissionDTO()
            {
                SubmittedFieldsDto = new DataFieldDTO[1]
                {
                  new DataFieldDTO() {Data = expectedData, Name = "testField2"}
                }, 
                UserId = 1
            };

            //Action
            _controller.DeliverTask(1, dto);
            var actualTask = _storageManager.GetTask(1);
            var actualData = actualTask.DataFields.SelectMany(d => d.UserData).FirstOrDefault(u => u.UserID == 1).Data;
            
            //Assert
            Assert.AreEqual(expectedData, actualData);
        }
    }
}
