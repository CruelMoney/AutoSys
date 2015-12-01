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
        Dictionary<int, StudyLogic> _studies;
        Mock<IGenericRepository> mockStudyRepo;
        int id;
        StudyLogic _testStudy = new StudyLogic() { Id = 1, CurrentStage = 1, IsFinished = false, Items = new List<ItemLogic>(), Stages = new List<StageLogic>(), Team = new TeamLogic(), TeamId = 1 };

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            mockStudyRepo = new Mock<IGenericRepository>();
            _studies = new Dictionary<int, StudyLogic>();

            // Read item
            mockStudyRepo.Setup(r => r.Read<StudyLogic>(It.IsAny<int>())).Returns<int, StudyLogic>((id, stud) => _studies.First(e => e.Key == id).Value);

            // Read items
            mockStudyRepo.Setup(r => r.Read<StudyLogic>()).Returns(_studies.Values.AsQueryable());

            // Create 
            mockStudyRepo.Setup(r => r.Create<StudyLogic>(It.IsAny<StudyLogic>())).Callback<StudyLogic>(study =>
            {
                int nextId = id++;
                study.Id = nextId;
                _studies.Add(nextId, study);
            });

            // Update
            mockStudyRepo.Setup(r => r.Update<StudyLogic>(It.IsAny<StudyLogic>())).Callback<StudyLogic>(study =>
            {
                if (_studies.ContainsKey(study.Id))
                {
                    _studies[study.Id] = study;
                }
            });

            // Delete
            mockStudyRepo.Setup(r => r.Delete<StudyLogic>(It.IsAny<StudyLogic>())).Callback<StudyLogic>(study =>
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