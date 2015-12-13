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
        Dictionary<int, StudyTask> _tasks;
     
        int id;

        Study _testStudy;

        StudyStorageManager testStudyStorageManager;
       
        TaskStorageManager testTaskStorageManager;
     

        StudyOverviewController testController;
     

        [TestInitialize]
        public void InitializeTests()
        {
            var user1 = new User() { ID = 1 };
            var user2 = new User() { ID = 2 };
            var user3 = new User() { ID = 3 };
            var user4 = new User() { ID = 4 };
            var finishedData = new UserData() { Data = new List<StoredString>() { new StoredString() { Value = "finished" }} };
            var userData2 = new UserData() { };
            var userData3 = new UserData() { };
            var userData4 = new UserData() { };

           
            var user1 = new User() { Id = 1 };
            var user2 = new User() { Id = 2 };

            var team1 = new Team() { Users = new List<User> { user1, user2 } };


            var dataString1 = new StoredString() { Value = "someData" };
            var datastring2 = new StoredString() { Value = "someOtherData" };
            var nullString = new StoredString() { Value = null };
            

            var userData1 = new UserData() { UserID = 1, Data = new List<StoredString>() {dataString1, datastring2 } };
            var userData2 = new UserData() { UserID = 2, Data = new List<StoredString>() { nullString, nullString } };
            var userData3 = new UserData() { UserID = 2, Data = new List<StoredString>() { dataString1, datastring2 } };
            var userData4 = new UserData() { UserID = 1, Data = new List<StoredString>() { nullString, nullString } };

            var dataField = new DataField() { UserData = new List<UserData> { userData1, userData3 } };
            var dataField2 = new DataField() { UserData = new List<UserData> { userData2, userData4 } };

            var task1 = new StudyTask() { DataFields = new List<DataField>() { dataField}, Users = new List<User> { user1, user2 } };
            var task2 = new StudyTask() { DataFields = new List<DataField>() { dataField2 }, Users = new List<User> { user1, user2 } };


            var userstudy1 = new UserStudies { User = user1 };
            var userstudy2 = new UserStudies { User = user2 };

            var stage1 = new Stage() { Name = "stage1", Id = 1, StudyID = 1, Users = new List<UserStudies> {userstudy1, userstudy2}, Tasks = new List<StudyTask>() { task1, task2 } };
            var stage2 = new Stage() { Name = "stage2", Id = 2, StudyID = 1, Users = new List<UserStudies> {userstudy1, userstudy2}, Tasks = new List<StudyTask>() { task1, task2 } };

            id = 1;
            mockStudyRepo = new Mock<IGenericRepository>();
            

            _testStudy = new Study() { Id = 1, CurrentStageID = 1, IsFinished = false, Items = new List<Item>(), Stages = new List<Stage>() {stage1, stage2 }, Team = team1};

            _studies = new Dictionary<int, Study> { { 1, _testStudy } };
          

            testStudyStorageManager = new StudyStorageManager(mockStudyRepo.Object);
         
            testController = new StudyOverviewController(testStudyStorageManager,testTaskStorageManager);


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
            //Action
            var result = testController.GetUserIDs(_testStudy);
        

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(2, result[1]);         
        }

        [TestMethod]
        public void TestOverviewCountAmountOfStages()
        {

            Assert.AreEqual(2, testController.GetStages(_testStudy).Length);
        }

        [TestMethod]
        public void TestOverviewCompletedTasks()
        {

            

            foreach(var stage in _testStudy.Stages)
            {
                if (stage.Name.Equals("stage1"))
                {
                    Assert.AreEqual(2, testController.GetCompletedTasks(stage).Count);
                }
                else
                {
                    Assert.AreEqual(2, testController.GetCompletedTasks(stage).Count);
                }
                
            }           
        }

        [TestMethod]
        public void TestOverviewIncompleteTasks()
        {

            foreach(var stage in _testStudy.Stages)
            {
                if (stage.Name.Equals("stage1"))
                {
                    Assert.AreEqual(2, testController.GetIncompleteTasks(stage).Count);
                }
                else
                {
                    Assert.AreEqual(2, testController.GetIncompleteTasks(stage).Count);
                }               
            }
        }

        [TestMethod]
        public void TestOverviewCurrentStage()
        {
            Assert.AreEqual(1, testController.GetCurrentStage(_testStudy).Id);
        }


     
    }
}
