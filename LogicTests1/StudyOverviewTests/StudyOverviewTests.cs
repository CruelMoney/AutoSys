using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyOverview;
using StudyConfigurationServer.Models;
using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using Moq;
using System.Linq;


namespace LogicTests1.StudyOverviewTests
{
    [TestClass]
    public class StudyOverviewTests
    {
       

        Dictionary<int, Study> _studies;
        Dictionary<int, StudyTask> _studyTasks;

        Mock<IGenericRepository> mockStudyRepo;
        List<IObserver<Study>> observer;
        int id;

        Study _testStudy;

        StudyStorageManager testStudyStorageManager;


        [TestInitialize]
        public void InitializeTests()
        {
            var user1 = new User() { Id = 1 };
            var user2 = new User() { Id = 2 };
            var user3 = new User() { Id = 3 };
            var user4 = new User() { Id = 4 };
            var finishedData = new UserData() { Data = new [] { "finished" } };
            var userData2 = new UserData() { };
            var userData3 = new UserData() { };
            var userData4 = new UserData() { };



            _testStudy =  new Study()
            {
                Id = 1,
                Team = new Team()
                {
                    UserIDs = new int[4] { 1, 2, 3, 4 },
                    Users = new List<User>() { user1, user2, user3, user4 }
                },
                CurrentStageID = 2,
                IsFinished = false,
                Items = new List<Item>(),
                Stages = new List<Stage>() {
            new Stage() {
                Name = "stage1" ,
                Id = 1, Tasks = new List<StudyTask>()
                {
                new StudyTask() {
                     UserIDs = new List<int>() {user1.Id,user2.Id,user3.Id},
                    DataFields = new List<DataField>()
                {
                    new DataField() {
                        UserData = new List<UserData>()
                        {
                            new UserData()
                            {
                                Data = new [] { "finished" },
                                UserID = user1.Id
                            },
                             new UserData()
                            {
                                Data = new [] { "" },
                                UserID = user2.Id
                            },
                              new UserData()
                            {
                                Data = new [] { "finished" },
                                UserID = user3.Id
                            }
                        } 
                       
                } } } } },
             new Stage() {
                Name = "stage2" ,
                Id = 2, Tasks = new List<StudyTask>()
                {
               new StudyTask() {
                   UserIDs = new List<int>() {user1.Id,user2.Id,user4.Id},
                   DataFields = new List<DataField>()
                {
                    new DataField() {
                        UserData = new List<UserData>()
                        {
                            new UserData()
                            {
                                Data = new [] { "" },
                                UserID = user1.Id
                            },
                             new UserData()
                            {
                                Data = new [] { "finished" },
                                UserID = user2.Id
                            },
                              new UserData()
                            {
                                Data = new [] { "" },
                                UserID = user4.Id
                            }
                        }
                } } } } } } };



            id = 1;
            mockStudyRepo = new Mock<IGenericRepository>();
            observer = new List<IObserver<Study>>();
            _studies = new Dictionary<int, Study>();
            _studyTasks = new Dictionary<int, StudyTask>();

            testStudyStorageManager = new StudyStorageManager(mockStudyRepo.Object, observer);

            // Read item
            mockStudyRepo.Setup(r => r.Read<Study>(It.IsAny<int>())).Returns<int>((id) => _studies.First(e => e.Key == id).Value);

            // Read items
            mockStudyRepo.Setup(r => r.Read<Study>()).Returns(_studies.Values.AsQueryable());

            // Create 
            mockStudyRepo.Setup(r => r.Create<Study>(It.IsAny<Study>())).Callback<Study>(study =>
            {
                int nextId = id++;
                study.Id = nextId;
                _studies.Add(nextId, study);
            });

            // Update
            mockStudyRepo.Setup(r => r.Update<Study>(It.IsAny<Study>())).Callback<Study>(study =>
            {
                if (_studies.ContainsKey(study.Id))
                {
                    _studies[study.Id] = study;

                }


            });

            // Delete
            mockStudyRepo.Setup(r => r.Delete<Study>(It.IsAny<Study>())).Callback<Study>(study =>
            {
                _studies.Remove(study.Id);
            });

        }
        
        [TestMethod]
        public void TestOverviewRetrieveAllUserIdsFromStudy()
        {
            testStudyStorageManager.SaveStudy(_testStudy);
            StudyOverviewController controller = new StudyOverviewController();
        
            Assert.AreEqual(4, controller.GetUserIDs(_testStudy).Length);
            Assert.AreEqual(2, controller.GetUserIDs(_testStudy)[1]);         
        }

        [TestMethod]
        public void TestOverviewCountAmountOfStages()
        {
            testStudyStorageManager.SaveStudy(_testStudy);
            StudyOverviewController controller = new StudyOverviewController();

            Assert.AreEqual(2, controller.GetStages(_testStudy).Length);
        }

        [TestMethod]
        public void TestOverviewCompletedTasks()
        {
            testStudyStorageManager.SaveStudy(_testStudy);
            StudyOverviewController controller = new StudyOverviewController();

            foreach(var stage in _testStudy.Stages)
            {
                if (stage.Name.Equals("stage1"))
                {
                    Assert.AreEqual(2, controller.GetCompletedTasks(stage).Count);
                }
                else
                {
                    Assert.AreEqual(1, controller.GetCompletedTasks(stage).Count);
                }
                
            }           
        }

        [TestMethod]
        public void TestOverviewIncompleteTasks()
        {
            testStudyStorageManager.SaveStudy(_testStudy);
            StudyOverviewController controller = new StudyOverviewController();

            foreach(var stage in _testStudy.Stages)
            {
                if (stage.Name.Equals("stage1"))
                {
                    Assert.AreEqual(1, controller.GetIncompleteTasks(stage).Count);
                }
                else
                {
                    Assert.AreEqual(2, controller.GetIncompleteTasks(stage).Count);
                }               
            }
        }

        [TestMethod]
        public void TestOverviewCurrentStage()
        {
            testStudyStorageManager.SaveStudy(_testStudy);
            StudyOverviewController controller = new StudyOverviewController();

            Assert.AreEqual(2, controller.GetCurrentStage(_testStudy).Id);
        }


     
    }
}
