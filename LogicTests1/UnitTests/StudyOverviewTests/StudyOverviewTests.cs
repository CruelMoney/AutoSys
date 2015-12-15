#region Using

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.StudyOverview;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StudyOverviewTests
{
    [TestClass]
    public class StudyOverviewTests
    {
        private int _id;
        private Mock<IGenericRepository> _mockStudyRepo;
        private Dictionary<int, Study> _studies;
        private Dictionary<int, StudyTask> _tasks;

        private StudyOverviewController _testController;

        private Study _testStudy;

        private StudyStorageManager _testStudyStorageManager;

        private TaskStorageManager _testTaskStorageManager;


        [TestInitialize]
        public void InitializeTests()
        {
            var user1 = new User {ID = 1};
            var user2 = new User {ID = 2};

            var team1 = new Team {Users = new List<User> {user1, user2}};


            var dataString1 = new StoredString {Value = "someData"};
            var datastring2 = new StoredString {Value = "someOtherData"};
            var nullString = new StoredString {Value = null};


            var userData1 = new UserData {UserId = 1, Data = new List<StoredString> {dataString1, datastring2}};
            var userData2 = new UserData {UserId = 2, Data = new List<StoredString> {nullString, nullString}};
            var userData3 = new UserData {UserId = 2, Data = new List<StoredString> {dataString1, datastring2}};
            var userData4 = new UserData {UserId = 1, Data = new List<StoredString> {nullString, nullString}};

            var dataField = new DataField {UserData = new List<UserData> {userData1, userData3}};
            var dataField2 = new DataField {UserData = new List<UserData> {userData2, userData4}};

            var task1 = new StudyTask
            {
                DataFields = new List<DataField> {dataField},
                Users = new List<User> {user1, user2}
            };
            var task2 = new StudyTask
            {
                DataFields = new List<DataField> {dataField2},
                Users = new List<User> {user1, user2}
            };


            var userstudy1 = new UserStudies {User = user1};
            var userstudy2 = new UserStudies {User = user2};

            var stage1 = new Stage
            {
                Name = "stage1",
                IsCurrentStage = true,
                ID = 1,
                Users = new List<UserStudies> {userstudy1, userstudy2},
                Tasks = new List<StudyTask> {task1, task2}
            };
            var stage2 = new Stage
            {
                Name = "stage2",
                IsCurrentStage = false,
                ID = 2,
                Users = new List<UserStudies> {userstudy1, userstudy2},
                Tasks = new List<StudyTask> {task1, task2}
            };

            _id = 1;
            _mockStudyRepo = new Mock<IGenericRepository>();


            _testStudy = new Study
            {
                ID = 1,
                IsFinished = false,
                Items = new List<Item>(),
                Stages = new List<Stage> {stage1, stage2},
                Team = team1
            };

            _studies = new Dictionary<int, Study> {{1, _testStudy}};


            _testStudyStorageManager = new StudyStorageManager(_mockStudyRepo.Object);

            _testController = new StudyOverviewController(_testStudyStorageManager, _testTaskStorageManager);


            // Read item
            _mockStudyRepo.Setup(r => r.Read<Study>(It.IsAny<int>()))
                .Returns<int>(id => _studies.First(e => e.Key == id).Value);

            // Read items
            _mockStudyRepo.Setup(r => r.Read<Study>()).Returns(_studies.Values.AsQueryable());

            // Create 
            _mockStudyRepo.Setup(r => r.Create(It.IsAny<Study>())).Callback<Study>(study =>
            {
                var nextId = _id++;
                study.ID = nextId;
                _studies.Add(nextId, study);
            });

            // Update
            _mockStudyRepo.Setup(r => r.Update(It.IsAny<Study>())).Callback<Study>(study =>
            {
                if (_studies.ContainsKey(study.ID))
                {
                    _studies[study.ID] = study;
                }
            });

            // Delete
            _mockStudyRepo.Setup(r => r.Delete(It.IsAny<Study>()))
                .Callback<Study>(study => { _studies.Remove(study.ID); });
        }


        [TestMethod]
        public void TestOverviewRetrieveAllUserIdsFromStudy()
        {
            //Action
            var result = _testController.GetUserIDs(_testStudy);


            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void TestOverviewCountAmountOfStages()
        {
            Assert.AreEqual(2, _testController.GetStages(_testStudy).Length);
        }

        [TestMethod]
        public void TestOverviewCompletedTasks()
        {
            foreach (var stage in _testStudy.Stages)
            {
                if (stage.Name.Equals("stage1"))
                {
                    Assert.AreEqual(2, _testController.GetCompletedTasks(stage).Count);
                }
                else
                {
                    Assert.AreEqual(2, _testController.GetCompletedTasks(stage).Count);
                }
            }
        }

        [TestMethod]
        public void TestOverviewIncompleteTasks()
        {
            foreach (var stage in _testStudy.Stages)
            {
                if (stage.Name.Equals("stage1"))
                {
                    Assert.AreEqual(2, _testController.GetIncompleteTasks(stage).Count);
                }
                else
                {
                    Assert.AreEqual(2, _testController.GetIncompleteTasks(stage).Count);
                }
            }
        }

        [TestMethod]
        public void TestOverviewCurrentStage()
        {
            Assert.AreEqual(1, _testController.GetCurrentStage(_testStudy).ID);
        }
    }
}