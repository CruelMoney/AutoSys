﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.StudyOverview;
using StudyConfigurationServer.Models;
using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using Moq;
using System.Linq;
using System.Data.Entity;
using LogicTests1.IntegrationTests.DBInitializers;

namespace LogicTests1.StudyOverviewTests
{
    [TestClass]
    public class StudyOverviewTests
    {
       

        Dictionary<int, Study> _studies;
        Dictionary<int, StudyTask> _studyTasks;
       

        int id;

        Study _testStudy;

        StudyStorageManager testStudyStorageManager;
        Mock<IGenericRepository> mockStudyRepo;
       
     

        [TestInitialize]
        public void InitializeTests()
        {

            _studies = new Dictionary<int, Study>();
            _studyTasks = new Dictionary<int, StudyTask>();
           
            var task = new StudyTask() { };
           

            var user1 = new User() { Id = 1 };
            var user2 = new User() { Id = 2 };

            var team1 = new Team() { Users = new List<User> { user1, user2 } };
            
            
            var dataField = new DataField() { }



            var userstudy1 = new UserStudies { User = user1 };
            var userstudy2 = new UserStudies { User = user2 };

            var stage1 = new Stage() { Name = "stage1", Id = 1, StudyID = 1, Users = new List<UserStudies> {userstudy1, userstudy2}, Tasks = new List<StudyTask>()};
            var stage2 = new Stage() { Name = "stage2", Id = 2, StudyID = 1, Users = new List<UserStudies> {userstudy1, userstudy2} };

            id = 1;
            mockStudyRepo = new Mock<IGenericRepository>();
            

            _testStudy = new Study() { Id = 1, CurrentStageID = 1, IsFinished = false, Items = new List<Item>(), Stages = new List<Stage>() {stage1, stage2 }, Team = team1};
           
            testStudyStorageManager = new StudyStorageManager(mockStudyRepo.Object);

            mockTask.Setup(r => r.IsFinished(user1.Id)).Returns(true);

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
        
            Assert.AreEqual(2, controller.GetUserIDs(_testStudy).Length);
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
