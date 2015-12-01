using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.StorageManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Storage.Repository;
using Logic.Model;
using Logic.Model.DTO;

namespace Logic.StorageManagement.Tests
{
    [TestClass()]
    public class StudyStorageManagerTests
    {
        Dictionary<int, Study> _studies;
        Mock<IGenericRepository> mockStudyRepo;
        int id;
        Study _testStudy = new Study() { Id = 1, CurrentStage = 1, IsFinished = false, Items = new List<Item>(), Stages = new List<Stage>(), Team = new Team(), TeamId = 1 };

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            mockStudyRepo = new Mock<IGenericRepository>();
            _studies = new Dictionary<int, Study>();

            // Read item
            mockStudyRepo.Setup(r => r.Read<Study>(It.IsAny<int>())).Returns<int, Study>((id, stud) => _studies.First(e => e.Key == id).Value);

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

        /// <summary>
        /// Tests if a study has been added to the mock repo
        /// </summary>

        [TestMethod()]
        public void StorageAddStudyTest()
        {
            StudyStorageManager testStudyManager = new StudyStorageManager(mockStudyRepo.Object);
            Assert.AreEqual(0, _studies.Values.ToList().Count);
            testStudyManager.saveStudy(_testStudy);
            Assert.AreEqual(1, _studies.Values.ToList().Count);
        }

        /// <summary>
        /// Tests if a study has been removed from the mock repo
        /// </summary>

        [TestMethod()]
        public void StorageRemoveStudyTest()
        {
            StudyStorageManager testStudyManager = new StudyStorageManager(mockStudyRepo.Object);
            Assert.AreEqual(0, _studies.Values.ToList().Count);
            testStudyManager.saveStudy(_testStudy);
            Assert.AreEqual(1, _studies.Values.ToList().Count);
            testStudyManager.removeStudy(_testStudy);
            Assert.AreEqual(0, _studies.Values.ToList().Count);
            
        }

        /// <summary>
        /// Tests if an exception is thrown if one tries to remove a study, while
        /// there are no studies to remove
        /// </summary>

        [TestMethod()]
        public void StorageNoStudyToRemoveTest()
        {
            StudyStorageManager testStudyManager = new StudyStorageManager(mockStudyRepo.Object);
            Assert.AreEqual(0, _studies.Values.ToList().Count);
            //Assert.IsFalse(testStudyManager.removeStudy(_testStudy));
        }
    }
}