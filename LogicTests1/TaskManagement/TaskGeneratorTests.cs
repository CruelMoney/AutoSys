using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StudyConfigurationServer.Logic.TaskManagement;
using StudyConfigurationServer.Models;

namespace LogicTests1.TaskManagement
{
    [TestClass()]
    public class TaskGeneratorTests
    {
        Stage testStage;
        List<Item> items;
        TaskGenerator _taskGenerator;
        Item testItem1;
        Item testItem2;
        Item testItem3;
        DataField.DataType expectedDataType = It.IsAny<DataField.DataType>();

        [TestInitialize]
        public void SetupStudy()
        {
            _taskGenerator = new TaskGenerator();

             testItem1 = new Item(Item.ItemType.Book, new Dictionary<Item.FieldType, string>());
             testItem2 = new Item(Item.ItemType.Article, new Dictionary<Item.FieldType, string>());
             testItem3 = new Item(Item.ItemType.PhDThesis, new Dictionary<Item.FieldType, string>());

            items = new List<Item>() {testItem1,testItem2,testItem3};
          
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

            testStage = new Stage() {Id = 1, Name = "stage1", Criteria = new List<Criteria>(){testCriteria}, StageType = StudyTask.Type.Review};
            var testStage2 = new Stage() { Id = 2, Name = "stage2" };
            
        }

        [TestMethod]
        public void TestGenerateReviewTasks()
        {
            //Arrange
            
            //Action
            var result = _taskGenerator.GenerateReviewTasks(items, testStage).ToList();

            //Assert 
            Assert.AreEqual(3, result.Count());

            //Assert tasks
            for (int i=0; i < result.Count; i++)
            {
                Assert.AreEqual(items[i], result[i].Paper);
                Assert.AreEqual(testStage.StageType, result[i].TaskType);
                Assert.AreEqual(testStage, result[i].Stage);
                Assert.AreEqual(1, result[i].DataFields.Count);
                Assert.AreEqual("expectedDescription", result[i].DataFields[0].Description);
                Assert.AreEqual(expectedDataType, result[i].DataFields[0].FieldType);
                Assert.AreEqual("expectedName", result[i].DataFields[0].Name);
                Assert.AreEqual(0, result[i].DataFields[0].UserData.Count);
            }

            }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestTaskGeneratorStageInvalidType()
        {
            //Action
            _taskGenerator.GenerateReviewTasks(items, new Stage() {StageType = StudyTask.Type.Conflict}).ToList();
        }

      
    }
}