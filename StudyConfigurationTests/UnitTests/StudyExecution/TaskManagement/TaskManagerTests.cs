#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage.Repository;
using StudyConfigurationServer.Logic.StudyExecution.TaskManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyConfiguration.TaskManagement
{
    [TestClass]
    public class TaskManagerTests
    {
        private Mock<IGenericRepository> _dbMock;
        private List<Item> _items;
        private TaskManager _manager;
        private StudyTask _task1;
        private StudyTask _task2;
        private StudyTask _task3;
        private Item _testItem1;
        private Item _testItem2;
        private Item _testItem3;


        [TestInitialize]
        public void Setup()
        {
            var item1Data = new Dictionary<FieldType, string>
            {
                {new FieldType {Type = FieldType.TypEField.Title}, "title"}
            };

            _testItem1 = new Item(Item.ItemType.Book, item1Data);
            _testItem2 = new Item(Item.ItemType.Book, item1Data);
            _testItem3 = new Item(Item.ItemType.Book, item1Data);

            _items = new List<Item> {_testItem1, _testItem2, _testItem1};
            
            var testUser1 = new User {Name = "chris"};
            var testUser2 = new User {Name = "ramos"};
            var testUser3 = new User {Name = "kathrin"};
            var testUser4 = new User {Name = "emil"};
            var testUser5 = new User {Name = "user1"};
            var testUser6 = new User {Name = "user2"};
            var testUser7 = new User {Name = "user3"};
            var testUser8 = new User {Name = "user4"};

            var testTeam1 = new Team
            {
                Name = "team1",
                Users = new List<User> {testUser1, testUser2, testUser3, testUser4}
            };
            var testTeam2 = new Team
            {
                Name = "team2",
                Users = new List<User> {testUser5, testUser6, testUser7, testUser8}
            };
            var testTeam3 = new Team
            {
                Name = "team3",
                Users = new List<User> {testUser1, testUser6, testUser3, testUser8}
            };

            var expectedUserData1 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = "2015"}},
                UserId = 1
            };
            var expectedUserData2 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = "2015"}},
                UserId = 2
            };
            var expectedUserData3 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = "2015"}},
                UserId = 2
            };

            var emptyUserData1 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = null}},
                UserId = 1
            };
            var emptyUserData2 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = null}},
                UserId = 2
            };

            //Finished
            _task1 = new StudyTask
            {
                DataFields = new List<DataField>
                {
                    new DataField
                    {
                        Name = "Year",
                        UserData = new List<UserData> {expectedUserData1, expectedUserData2}
                    }
                },
                Users = new List<User> {testUser1, testUser2},
                TaskType = StudyTask.Type.Review,
                IsEditable = true,
                Paper = _testItem1
            };

            //Unfinished
            _task2 = new StudyTask
            {
                DataFields = new List<DataField>
                {
                    new DataField
                    {
                        Name = "Year",
                        UserData = new List<UserData> {emptyUserData1, expectedUserData3}
                    }
                },
                Users = new List<User> {testUser1, testUser2},
                TaskType = StudyTask.Type.Review,
                IsEditable = true,
                Paper = _testItem2
            };

            //Not Editable
            _task3 = new StudyTask
            {
                DataFields = new List<DataField>
                {
                    new DataField
                    {
                        Name = "Year",
                        UserData = new List<UserData> {expectedUserData1}
                    }
                },
                Users = new List<User> {testUser1},
                TaskType = StudyTask.Type.Review,
                IsEditable = false,
                Paper = _testItem3
            };


            _dbMock = new Mock<IGenericRepository>();
            _dbMock.Setup(t => t.Read<StudyTask>(1))
                .Returns(_task1);
            _dbMock.Setup(t => t.Read<StudyTask>(3))
                .Returns(_task3);

            _dbMock.Setup(t => t.Update(_task1))
                .Returns(true);

            _manager = new TaskManager(_dbMock.Object);
        }

        [TestMethod]
        public void TestDeliverTask()
        {
            //Arrange
            var taskDto = new TaskSubmissionDto
            {
                UserId = 1,
                SubmittedFields = new[]
                {
                    new DataFieldDto {Data = new[] {"updatedData"}, Name = "Year"}
                }
            };

            //Action
            _manager.DeliverTask(1, taskDto);

            //Assert
            _dbMock.Verify(d => d.Read<StudyTask>(1), Times.Once);
            _dbMock.Verify(d => d.Update(It.IsAny<StudyTask>()), Times.Once);
        }

        [ExpectedException(typeof (ArgumentException))]
        [TestMethod]
        public void TestDeliverNotEditableTask()
        {
            //Arrange
            var taskDto = new TaskSubmissionDto
            {
                UserId = 1,
                SubmittedFields = new[]
                {
                    new DataFieldDto {Data = new[] {"updatedData"}, Name = "Year"}
                }
            };

            //Action
            _manager.DeliverTask(3, taskDto);

            //Assert
            _dbMock.Verify(d => d.Read<StudyTask>(3), Times.Once);
            _dbMock.Verify(d => d.Update(It.IsAny<StudyTask>()), Times.Never);
        }

        [ExpectedException(typeof (ArgumentException))]
        [TestMethod]
        public void TestDeliverInvaliduser()
        {
            //Arrange
            var taskDto = new TaskSubmissionDto
            {
                UserId = 100,
                SubmittedFields = new[]
                {
                    new DataFieldDto {Data = new[] {"updatedData"}, Name = "Year"}
                }
            };

            //Action
            _manager.DeliverTask(1, taskDto);

            //Assert
            _dbMock.Verify(d => d.Read<StudyTask>(1), Times.Once);
            _dbMock.Verify(d => d.Update(It.IsAny<StudyTask>()), Times.Never);
        }

        [TestMethod]
        public void TestGenerateReviewTasks()
        {
            //Arrange

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
            var result = _manager.GenerateReviewTasks(_items, criteria);

            //Assert
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void TestGetExcludedItems()
        {
            //Arrange

            var testCriteria = new Criteria
            {
                DataType = DataField.DataType.String,
                Description = "expectedDescription",
                Name = "Year",
                DataMatch = new List<StoredString> {new StoredString("1999")},
                Rule = Criteria.CriteriaRule.SmallerThan
            };

            var criteria = new List<Criteria> {testCriteria};

            var tasks = new List<StudyTask> {_task1, _task2, _task3};

            //Action
            var result = _manager.GetExcludedItems(tasks, criteria);

            //Assert
            Assert.AreEqual(2, result.Count());
        }
    }
}