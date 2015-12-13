using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;

namespace LogicTests1.StorageManagement
{
    [TestClass()]
    public class StudyStorageManagerTests
    {
        Dictionary<int, Study> _studies;
        Mock<IGenericRepository> mockStudyRepo;
        List<IObserver<Study>> observer;
        int id;
        Study _testStudy = new Study() { ID = 1,  IsFinished = false, Items = new List<Item>(), Stages = new List<Stage>() };
        StudyStorageManager testStudyStorageManager;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            mockStudyRepo = new Mock<IGenericRepository>();
            observer = new List<IObserver<Study>>();
            _studies = new Dictionary<int, Study>();
            testStudyStorageManager = new StudyStorageManager(mockStudyRepo.Object);

            // Read item
            mockStudyRepo.Setup(r => r.Read<Study>(It.IsAny<int>())).Returns<int>((id) => _studies.First(e => e.Key == id).Value);

            // Read items
            mockStudyRepo.Setup(r => r.Read<Study>()).Returns(_studies.Values.AsQueryable());

            // Create 
            mockStudyRepo.Setup(r => r.Create<Study>(It.IsAny<Study>())).Callback<Study>(study =>
            {
                int nextId = id++;
                study.ID = nextId;
                _studies.Add(nextId, study);
            });

            // Update
            mockStudyRepo.Setup(r => r.Update<Study>(It.IsAny<Study>())).Callback<Study>(study =>
            {
                if (_studies.ContainsKey(study.ID))
                {
                    _studies[study.ID] = study;   
                    
                }

                
            });

            // Delete
            mockStudyRepo.Setup(r => r.Delete<Study>(It.IsAny<Study>())).Callback<Study>(study =>
            {
                _studies.Remove(study.ID);
            });

        }

        /// <summary>
        /// Tests if a study has been added to the mock repo
        /// </summary>

        [TestMethod()]
        public void TestStorageAddStudy()
        {
            testStudyStorageManager.SaveStudy(_testStudy);
            Assert.AreEqual(1, _studies.Values.ToList().Count);
        }

        /// <summary>
        /// Tests get on a Study in the mock repo
        /// </summary>

        [TestMethod]
        public void TestStorageGetStudy()
        {
            testStudyStorageManager.SaveStudy(_testStudy);
            Assert.AreEqual(_testStudy, testStudyStorageManager.GetStudy(1));
        }

        /// <summary>
        /// Tests get on all studies in the mock repo
        /// </summary>

        public void TestStorageGetAllStudies()
        {
            testStudyStorageManager.SaveStudy(_testStudy);
            var _testStudy2 = new Study();
            testStudyStorageManager.SaveStudy(_testStudy2);
            Assert.AreEqual(2, testStudyStorageManager.GetAllStudies().Count());
        }

        /// <summary>
        /// Tests if a study has been removed from the mock repo
        /// </summary>

        [TestMethod()]
        public void TestStorageRemoveStudy()
        {
            testStudyStorageManager.SaveStudy(_testStudy);
            Assert.AreEqual(1, _studies.Values.ToList().Count);
            testStudyStorageManager.RemoveStudy(_testStudy.ID);
            Assert.AreEqual(0, _studies.Values.ToList().Count);
            
        }

        /// <summary>
        /// Tests if an exception is thrown and Remove() returns false if one tries to remove a study, while
        /// there are no studies to remove
        /// </summary>

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestStorageNoStudyToRemove()
        {
            Assert.AreEqual(0, _studies.Values.ToList().Count);
            Assert.IsFalse(testStudyStorageManager.RemoveStudy(_testStudy.ID));
        }

        /// <summary>
        /// Tests if a study is updated
        /// </summary>

        [TestMethod]
        public void TestStorageUpdateStudy()
        {
            var stageTestStudy = new Study() { ID = 1, Name = "study"};
            testStudyStorageManager.SaveStudy(stageTestStudy);
            stageTestStudy.Name = "updatedname";
            testStudyStorageManager.UpdateStudy(stageTestStudy);
            Assert.AreEqual("updatedname", testStudyStorageManager.GetStudy(1).Name);
            
        }
    }
}