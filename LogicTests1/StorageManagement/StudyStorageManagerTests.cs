using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.StorageManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Storage.Repository;
using Logic.Model.Data;
using Logic.Model;
using Logic.Model.DTO;

namespace Logic.StorageManagement.Tests
{
    [TestClass()]
    public class StudyStorageManagerTests
    {
        // Dictionary<int, StoredStudy> _studies;
        Mock<IRepository> mockStudyRepo;
        Dictionary<int, StoredStudy> _storedStudies;
        int id = 1;
        TeamLogic _testTeam = new TeamLogic();
        List<ItemLogic> _testList = new List<ItemLogic>();

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            // _studies = new Dictionary<int, StoredStudy>();
            mockStudyRepo = new Mock<IRepository>();
            _storedStudies = new Dictionary<int, StoredStudy>();

            // Read item
            mockStudyRepo.Setup(r => r.Read<StoredStudy>(It.IsAny<int>())).Returns<int, StoredStudy>((id, stud) => _storedStudies.First(e => e.Key == id).Value);

            // Read items
            mockStudyRepo.Setup(r => r.Read<StoredStudy>()).Returns(_storedStudies.Values.AsQueryable());

            // Create 
            mockStudyRepo.Setup(r => r.Create<StoredStudy>(It.IsAny<StoredStudy>())).Callback<StoredStudy>(study =>
            {
                int nextId = id++;
                study.Id = nextId;
                _storedStudies.Add(nextId, study);
            });

            // Update
            mockStudyRepo.Setup(r => r.Update<StoredStudy>(It.IsAny<StoredStudy>())).Callback<StoredStudy>(study =>
            {
                if (_storedStudies.ContainsKey(study.Id))
                {
                    _storedStudies[study.Id] = study;
                }
            });

            // Delete
            mockStudyRepo.Setup(r => r.Delete<StoredStudy>(It.IsAny<StoredStudy>())).Callback<StoredStudy>(study =>
            {
                _storedStudies.Remove(study.Id);
            });

        }

        /// <summary>
        /// Tests if a study has been added to the mock repo
        /// </summary>

        [TestMethod()]
        public void AddStudyTest()
        {
            StudyStorageManager TestManager = new StudyStorageManager(mockStudyRepo.Object);
            var TestStudy = TestManager.saveStudy("TestStudy", _testTeam, _testList);
            Assert.AreEqual(1, _storedStudies.Values.ToList().Count);
            Assert.AreEqual(TestStudy.Name, "TestStudy");
            Assert.AreEqual(TestStudy.Team, _testTeam);
            Assert.AreEqual(TestStudy.studyData, _testList);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests if a study has been removed from the mock repo
        /// </summary>

        [TestMethod()]
        public void RemoveStudyTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests if an exception is thrown if one tries to remove a study, while
        /// there are no studies to remove
        /// </summary>

        [TestMethod()]
        public void NoStudyToRemoveTest()
        {
            throw new NotImplementedException();
        }
    }
}