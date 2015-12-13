using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement;
using StudyConfigurationServer.Models;

namespace LogicTests1.StudyConfiguration.TaskManagement
{
    [TestClass()]
    public class TaskGeneratorTests
    {
        Stage testStage1;
        List<Item> items;
        TaskGenerator _taskGenerator;
        Item testItem1;
        Item testItem2;
        Item testItem3;
        DataField.DataType expectedDataType = It.IsAny<DataField.DataType>();
        StudyTask conflictingTask;
        User user1;
        User user2;
        UserData expectedUserData1;
        UserData expectedUserData2;


        [TestInitialize]
        public void SetupStudy()
        {
            _taskGenerator = new TaskGenerator();

             testItem1 = new Item(Item.ItemType.Book, new Dictionary<FieldType, string>());
             testItem2 = new Item(Item.ItemType.Article, new Dictionary<FieldType, string>());
             testItem3 = new Item(Item.ItemType.PhDThesis, new Dictionary<FieldType, string>());

             items = new List<Item>() {testItem1,testItem2,testItem3};
          
             var user1 = new User() {ID = 1};
             var user2 = new User() { ID = 2 };

            var testCriteria = new Criteria()
            {
                DataType = expectedDataType,
                Description = "expectedDescription",
                Name = "expectedName",
                TypeInfo = new string[1] {"expectedInfo"}
            };

            var testCriteria2 = new Criteria()
            {
                DataType = expectedDataType,
                Description = "expectedDescription2",
                Name = "expectedName2",
                TypeInfo = new string[1] { "expectedInfo2" }
            };

            testStage1 = new Stage() {ID = 1, Name = "stage1", Criteria = new List<Criteria>(){testCriteria}, CurrentTaskType = StudyTask.Type.Review};
            var testStage2 = new Stage() { ID = 2, Name = "stage2" };

            
            expectedUserData1 = new UserData() {Data = new List<StoredString>() { new StoredString() { Value = "conflictingData1" }}, UserID = 1};
            expectedUserData2 = new UserData() {Data = new List<StoredString>() { new StoredString() { Value = "conflictingData2" }}, UserID = 2};

            conflictingTask = new StudyTask()
            {
                DataFields = new List<DataField>()
                {
                    new DataField()
                    {
                        UserData = new List<UserData>()
                        {
                           expectedUserData1, expectedUserData2
                        },
                        Description = "dataFieldDescription1",
                        FieldType = expectedDataType,
                        Name = "dataFieldName1"
                    }
                },
                Paper = testItem1,
                TaskType = StudyTask.Type.Review,
                Users = new List<User>() { user1 ,user2}
            };

        }

        [TestMethod]
        public void TestGenerateReviewTasks()
        {
            //Arrange
            
            //Action
            var result = items.Select(item => _taskGenerator.GenerateReviewTask(item, testStage1.Criteria)).ToList();
            
            //Assert 
            Assert.AreEqual(3, result.Count());

            //Assert tasks
            for (int i=0; i < result.Count; i++)
            {
                Assert.AreEqual(items[i], result[i].Paper);
                Assert.AreEqual(testStage1.CurrentTaskType, result[i].TaskType);
            
                Assert.AreEqual(1, result[i].DataFields.Count);
                Assert.AreEqual("expectedDescription", result[i].DataFields[0].Description);
                Assert.AreEqual(expectedDataType, result[i].DataFields[0].FieldType);
                Assert.AreEqual("expectedName", result[i].DataFields[0].Name);
                Assert.AreEqual(0, result[i].DataFields[0].UserData.Count);
            }

            }

        [TestMethod]
        public void TestGenerateValidateTasks()
        {
            //Arrange

            //Action
            var result = _taskGenerator.GenerateValidateTasks(conflictingTask);

            //Assert 
            Assert.AreEqual(items[0], result.Paper);
            Assert.AreEqual(StudyTask.Type.Conflict, result.TaskType);
            Assert.AreEqual(1, result.DataFields.Count);
            Assert.AreEqual("dataFieldDescription1", result.DataFields[0].Description);
            Assert.AreEqual(expectedDataType, result.DataFields[0].FieldType);
            Assert.AreEqual("dataFieldName1", result.DataFields[0].Name);

            //Assert userdata
            Assert.AreEqual(2, result.DataFields[0].ConflictingData.Count);
            Assert.AreEqual(expectedUserData1, result.DataFields[0].ConflictingData[0]);
            Assert.AreEqual(expectedUserData2, result.DataFields[0].ConflictingData[1]);
        }

      
    }
}