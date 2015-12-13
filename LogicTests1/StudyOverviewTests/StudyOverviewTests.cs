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
            var finishedData = new UserData() { Data = new List<StoredString>() { new StoredString() { Value = "finished" }} };
            var userData2 = new UserData() { };
            var userData3 = new UserData() { };
            var userData4 = new UserData() { };



         

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
