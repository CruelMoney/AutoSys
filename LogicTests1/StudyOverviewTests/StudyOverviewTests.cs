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
        int id;    
        Study _testStudy = new Study() { Id = 1,Team = new Team() {UserIDs = new int[4] { 1, 2, 3, 4 } }, CurrentStageID = 1, IsFinished = false, Items = new List<Item>(), Stages = new List<Stage>() };
        StudyStorageManager testStudyStorageManager;


        [TestInitialize]
        public void InitializeTests()
        {
            id = 1;
            mockStudyRepo = new Mock<IGenericRepository>();
            _studies = new Dictionary<int, Study>();
            _studyTasks = new Dictionary<int, StudyTask>();
            testStudyStorageManager = new StudyStorageManager(mockStudyRepo.Object);

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
        public void TestRetrieveAllUserIdsFromStudy()
        {
            testStudyStorageManager.SaveStudy(_testStudy);
            StudyOverviewController controller = new StudyOverviewController();

            Assert.AreEqual(4, controller.GetUserIDs(_testStudy).Length);
            
        }
        /*
        [TestMethod]
        public void Test()
        {

        }*/
    }
}
