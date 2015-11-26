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
        // Dictionary<int, StudyLogic> _studies;
        Mock<IRepository> mockStudyRepo;
        Dictionary<int, StudyLogic> _storedStudies;
        int id = 1;
        TeamLogic _testTeam = new TeamLogic();
        List<ItemLogic> _testList = new List<ItemLogic>();

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            // _studies = new Dictionary<int, StudyLogic>();
            mockStudyRepo = new Mock<IRepository>();
            _storedStudies = new Dictionary<int, StudyLogic>();

            // Read item
            mockStudyRepo.Setup(r => r.Read<StudyLogic>(It.IsAny<int>())).Returns<int, StudyLogic>((id, stud) => _storedStudies.First(e => e.Key == id).Value);

            // Read items
            mockStudyRepo.Setup(r => r.Read<StudyLogic>()).Returns(_storedStudies.Values.AsQueryable());

            // Create 
            mockStudyRepo.Setup(r => r.Create<StudyLogic>(It.IsAny<StudyLogic>())).Callback<StudyLogic>(study =>
            {
                int nextId = id++;
                study.Id = nextId;
                _storedStudies.Add(nextId, study);
            });

            // Update
            mockStudyRepo.Setup(r => r.Update<StudyLogic>(It.IsAny<StudyLogic>())).Callback<StudyLogic>(study =>
            {
                if (_storedStudies.ContainsKey(study.Id))
                {
                    _storedStudies[study.Id] = study;
                }
            });

            // Delete
            mockStudyRepo.Setup(r => r.Delete<StudyLogic>(It.IsAny<StudyLogic>())).Callback<StudyLogic>(study =>
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