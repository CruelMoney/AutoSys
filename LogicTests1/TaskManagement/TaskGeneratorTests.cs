using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Models;

namespace LogicTests1.TaskManagement
{
    [TestClass()]
    public class TaskGeneratorTests
    {
        Study testStudy;

        [TestInitialize]
        public void SetupStudy()
        {
            var testItem = new Item(Item.ItemType.Book, new Dictionary<Item.FieldType, string>());
            var testItem2 = new Item(Item.ItemType.Article, new Dictionary<Item.FieldType, string>());
            var testItem3 = new Item(Item.ItemType.PhDThesis, new Dictionary<Item.FieldType, string>());

            var testStage = new Stage() {Id = 1, Name = "stage1"};
            var testStage2 = new Stage() { Id = 2, Name = "stage2" };
            
            testStudy = new Study()
            {
                Items = new List<Item>() { testItem, testItem2, testItem3},
                Stages = new List<Stage>() { testStage,testStage2},
            };
        }

        [TestMethod()]
        public void TestGenerateTaskStage1()
        {
            
        }
    }
}