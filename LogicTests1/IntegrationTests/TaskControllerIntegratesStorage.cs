using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests1.IntegrationTests.DBInitializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;

namespace LogicTests1.IntegrationTests
{
    [TestClass]
    public class TaskManagerIntegratesStorage
    {
        TaskStorageManager _storageManager;
        TaskManager _manager;
        EntityFrameworkGenericRepository<StudyContext> _repo;
        List<User> users;
        List<Criteria> criteria; 

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

            _repo = new EntityFrameworkGenericRepository<StudyContext>();
            _storageManager = new TaskStorageManager(_repo);
            _manager = new TaskManager(_repo);
            var testItem = new Item(Item.ItemType.Book, new Dictionary<FieldType, string>());
            var testUser1 = new User() { Id = 1, Name = "chris" };
            var testUser2 = new User() { Id = 2, Name = "ramos" };
            var userData1 = new UserData() { UserID = 1, Data = new string[1] { "initialData" } };
            var userData2 = new UserData() { UserID = 2, Data = new string[1] { "" } };
            var userData3 = new UserData() { UserID = 2, Data = new string[1] };
            var dataFields1 = new List<DataField>() { new DataField() { UserData = new List<UserData>() { userData1 }, Name = "testField", Description = "testDescription" } };
            var dataFields2 = new List<DataField>() { new DataField() { UserData = new List<UserData>() { userData2, userData1 }, Name = "testField2", Description = "testDescription2" } };
            var dataFields3 = new List<DataField>() { new DataField() { UserData = new List<UserData>() { userData3, userData1 }, Name = "testField3", Description = "testDescription" } };

            users = new List<User>() {testUser1, testUser2};

            var testTask = new StudyTask()
            {
                Paper = testItem,
                DataFields = new List<DataField>(dataFields2),
                IsEditable = true
            };
            
            _storageManager.CreateTask(testTask);

            var testCriteria1 = new Criteria()
            {
                Name = "testCriteria",
                Description = "this is a test Criteria",
                Rule = Criteria.CriteriaRule.Contains,
                DataType = DataField.DataType.String,
                DataMatch = new string[] { "software" }

            };
            var testCriteria2 = new Criteria()
            {
                Name = "testCriteria2",
                Description = "this is a test Criteria",
                Rule = Criteria.CriteriaRule.Equals,
                DataType = DataField.DataType.Boolean,
                DataMatch = new string[] {"true"}
            };

            criteria = new List<Criteria>() {testCriteria1, testCriteria2};
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
            _manager.DeliverTask(1, dto);
            var actualTask = _storageManager.GetTask(1);
            var actualData = actualTask.DataFields.SelectMany(d => d.UserData).FirstOrDefault(u => u.UserID == 1).Data;
            
            //Assert
            Assert.AreEqual(expectedData, actualData);
        }

        [TestMethod]
        public void generateReviewTasks()
        {
            //Arrange
            var testItem1 = new Item(Item.ItemType.Book, new Dictionary<FieldType, string>());
            var  testItem2 = new Item(Item.ItemType.Article, new Dictionary<FieldType, string>());
            var testItem3 = new Item(Item.ItemType.PhDThesis, new Dictionary<FieldType, string>());

            var items = new List<Item>() { testItem1, testItem2, testItem3 };

            

            //Action
            var tasks =_manager.GenerateReviewTasks(items, users, criteria, Stage.Distribution.HundredPercentOverlap);
          
            //Assert
            foreach (var studyTask in tasks)
            {
                Assert.AreEqual(2, studyTask.Users.Count);
            }
        }
    }
}
