#region Using

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyConfiguration.TaskManagement
{
    [TestClass]
    public class TaskGeneratorTests
    {
        private StudyTask _conflictingTask;
        private UserData _expectedUserData1;
        private UserData _expectedUserData2;
        private List<Item> _items;
        private TaskGenerator _taskGenerator;
        private Stage _testStage1;
        private User _user1;
        private User _user2;


        [TestInitialize]
        public void SetupStudy()
        {
            _taskGenerator = new TaskGenerator();
        }

        [TestMethod]
        public void TestGenerateReviewTask()
        {
            //Arrange
            var item1Data = new Dictionary<FieldType, string>
            {
                {new FieldType {Type = FieldType.TypEField.Title}, "title1"}
            };

            var testItem1 = new Item(Item.ItemType.Book, item1Data);

            var testCriteria = new Criteria
            {
                DataType = DataField.DataType.String,
                Description = "expectedDescription",
                Name = "expectedName",
                DataMatch = new List<StoredString> {new StoredString("expectedDataMatch")},
                Rule = Criteria.CriteriaRule.Equals
            };

            var criteria = new List<Criteria> {testCriteria};

            //Action
            var result = _taskGenerator.GenerateReviewTask(testItem1, criteria);

            //Assert 
            Assert.AreEqual(testItem1, result.Paper);
            Assert.AreEqual("expectedName", result.DataFields[0].Name);
            Assert.AreEqual("expectedDescription", result.DataFields[0].Description);
            Assert.AreEqual(DataField.DataType.String, result.DataFields[0].FieldType);
            Assert.AreEqual(true, result.IsEditable);
        }


        [TestMethod]
        public void TestGenerateReviewTaskWithTypeInfo()
        {
            //Arrange
            var item1Data = new Dictionary<FieldType, string>
            {
                {new FieldType {Type = FieldType.TypEField.Title}, "title1"}
            };

            var testItem1 = new Item(Item.ItemType.Book, item1Data);

            var testCriteria = new Criteria
            {
                DataType = DataField.DataType.Enumeration,
                Description = "expectedDescription",
                Name = "expectedName",
                DataMatch = new List<StoredString> {new StoredString("expectedDataMatch")},
                Rule = Criteria.CriteriaRule.Equals,
                TypeInfo = new List<StoredString> {new StoredString("typeinfo1"), new StoredString("typeinfo2")}
            };

            var criteria = new List<Criteria> {testCriteria};

            //Action
            var result = _taskGenerator.GenerateReviewTask(testItem1, criteria);

            //Assert 
            Assert.AreEqual(testItem1, result.Paper);
            Assert.AreEqual("expectedName", result.DataFields[0].Name);
            Assert.AreEqual("expectedDescription", result.DataFields[0].Description);
            Assert.AreEqual(DataField.DataType.Enumeration, result.DataFields[0].FieldType);
            Assert.AreEqual(true, result.IsEditable);
            Assert.AreEqual("typeinfo1", result.DataFields[0].TypeInfo.ToList()[0].Value);
            Assert.AreEqual("typeinfo2", result.DataFields[0].TypeInfo.ToList()[1].Value);
        }

        [TestMethod]
        public void TestGenerateReviewTaskMultipleCriteria()
        {
            //Arrange
            var item1Data = new Dictionary<FieldType, string>
            {
                {new FieldType {Type = FieldType.TypEField.Title}, "title1"}
            };

            var testItem1 = new Item(Item.ItemType.Book, item1Data);

            var testCriteria = new Criteria
            {
                DataType = DataField.DataType.String,
                Description = "expectedDescription",
                Name = "expectedName",
                DataMatch = new List<StoredString> {new StoredString("expectedDataMatch")},
                Rule = Criteria.CriteriaRule.Equals
            };

            var testCriteria2 = new Criteria
            {
                DataType = DataField.DataType.Boolean,
                Description = "expectedDescription2",
                Name = "expectedName2",
                DataMatch = new List<StoredString> {new StoredString("true")},
                Rule = Criteria.CriteriaRule.Equals
            };

            var criteria = new List<Criteria> {testCriteria, testCriteria2};

            //Action
            var result = _taskGenerator.GenerateReviewTask(testItem1, criteria);

            //Assert 
            Assert.AreEqual(testItem1, result.Paper);
            Assert.AreEqual("expectedName", result.DataFields[0].Name);
            Assert.AreEqual("expectedDescription", result.DataFields[0].Description);
            Assert.AreEqual(DataField.DataType.String, result.DataFields[0].FieldType);
            Assert.AreEqual(true, result.IsEditable);
            Assert.AreEqual(testItem1, result.Paper);
            Assert.AreEqual("expectedName2", result.DataFields[1].Name);
            Assert.AreEqual("expectedDescription2", result.DataFields[1].Description);
            Assert.AreEqual(DataField.DataType.Boolean, result.DataFields[1].FieldType);
            Assert.AreEqual(true, result.IsEditable);
        }

        [TestMethod]
        public void TestGenerateReviewTaskWithItemData()
        {
            //Arrange
            var item1Data = new Dictionary<FieldType, string>
            {
                {new FieldType {Type = FieldType.TypEField.Title}, "title1"}
            };

            var testItem1 = new Item(Item.ItemType.Book, item1Data);

            var testCriteria = new Criteria
            {
                DataType = DataField.DataType.String,
                Description = "expectedDescription",
                Name = "Title",
                DataMatch = new List<StoredString> {new StoredString("expectedDataMatch")},
                Rule = Criteria.CriteriaRule.Equals
            };

            var criteria = new List<Criteria> {testCriteria};

            //Action
            var result = _taskGenerator.GenerateReviewTask(testItem1, criteria);

            //Assert 
            Assert.AreEqual(testItem1, result.Paper);
            Assert.AreEqual("Title", result.DataFields[0].Name);
            Assert.AreEqual("expectedDescription", result.DataFields[0].Description);
            Assert.AreEqual(DataField.DataType.String, result.DataFields[0].FieldType);
            Assert.AreEqual(false, result.IsEditable);
            Assert.AreEqual("title1", result.DataFields[0].UserData[0].Data[0].Value);
        }

        [TestMethod]
        public void TestGenerateValidateTask()
        {
            //Arrange
            var item1Data = new Dictionary<FieldType, string>
            {
                {new FieldType {Type = FieldType.TypEField.Title}, "title1"}
            };

            var testItem1 = new Item(Item.ItemType.Book, item1Data);

            _expectedUserData1 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = "conflictingData1"}},
                UserId = 1
            };
            _expectedUserData2 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = "conflictingData2"}},
                UserId = 2
            };

            _conflictingTask = new StudyTask
            {
                DataFields = new List<DataField>
                {
                    new DataField
                    {
                        UserData = new List<UserData>
                        {
                            _expectedUserData1,
                            _expectedUserData2
                        },
                        Description = "conflictingFieldDescription",
                        FieldType = DataField.DataType.String,
                        Name = "conflictingField"
                    }
                },
                Paper = testItem1,
                TaskType = StudyTask.Type.Review,
                Users = new List<User> {_user1, _user2}
            };


            var testCriteria = new Criteria
            {
                DataType = DataField.DataType.String,
                Description = "expectedDescription",
                Name = "Title",
                DataMatch = new List<StoredString> {new StoredString("expectedDataMatch")},
                Rule = Criteria.CriteriaRule.Equals
            };

            var criteria = new List<Criteria> {testCriteria};

            //Action
            var result = _taskGenerator.GenerateValidateTasks(_conflictingTask);

            //Assert 
            Assert.AreEqual(testItem1, result.Paper);
            Assert.AreEqual("conflictingField", result.DataFields[0].Name);
            Assert.AreEqual("conflictingFieldDescription", result.DataFields[0].Description);
            Assert.AreEqual(DataField.DataType.String, result.DataFields[0].FieldType);
            Assert.AreEqual(true, result.IsEditable);
            Assert.AreEqual(0, result.DataFields[0].UserData.Count);
            Assert.AreEqual("conflictingData1", result.DataFields[0].ConflictingData[0].Data[0].Value);
            Assert.AreEqual("conflictingData2", result.DataFields[0].ConflictingData[1].Data[0].Value);
            Assert.AreEqual(1, result.DataFields[0].ConflictingData[0].UserId);
            Assert.AreEqual(2, result.DataFields[0].ConflictingData[1].UserId);
        }
    }
}