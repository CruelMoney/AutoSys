#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StorageManagement
{
    [TestClass]
    public class StudyStorageManagerTests
    {
        private int _id;
        private Mock<IGenericRepository> _mockStudyRepo;
        private Dictionary<int, Study> _studies;

        private readonly Study _testStudy = new Study
        {
            ID = 1,
            IsFinished = false,
            Items = new List<Item>(),
            Stages = new List<Stage>()
        };

        private StudyStorageManager _testStudyStorageManager;
        private TaskStorageManager _testTaskStorageManager;

        [TestInitialize]
        public void InitializeRepo()
        {
            _id = 1;
            _mockStudyRepo = new Mock<IGenericRepository>();


            _studies = new Dictionary<int, Study>();

            _testStudyStorageManager = new StudyStorageManager(_mockStudyRepo.Object);


            // Read item
            _mockStudyRepo.Setup(r => r.Read<Study>(It.IsAny<int>()))
                .Returns<int>(id => _studies.First(e => e.Key == id).Value);

            // Read items
            _mockStudyRepo.Setup(r => r.Read<Study>()).Returns(_studies.Values.AsQueryable());

            // Create 
            _mockStudyRepo.Setup(r => r.Create(It.IsAny<Study>())).Callback<Study>(study =>
            {
                var nextId = _id++;
                study.ID = nextId;
                _studies.Add(nextId, study);
            });

            // Update
            _mockStudyRepo.Setup(r => r.Update(It.IsAny<Study>())).Callback<Study>(study =>
            {
                if (_studies.ContainsKey(study.ID))
                {
                    _studies[study.ID] = study;
                }
            });

            // Delete
            _mockStudyRepo.Setup(r => r.Delete(It.IsAny<Study>()))
                .Callback<Study>(study => { _studies.Remove(study.ID); });
        }

        /// <summary>
        ///     Tests if a study has been added to the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageAddStudy()
        {
            _testStudyStorageManager.Save(_testStudy);
            Assert.AreEqual(1, _studies.Values.ToList().Count);
        }

        /// <summary>
        ///     Tests get on a Study in the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageGetStudy()
        {
            _testStudyStorageManager.Save(_testStudy);
            Assert.AreEqual(_testStudy, _testStudyStorageManager.Get(1));
        }

        /// <summary>
        ///     Tests get on all studies in the mock repo
        /// </summary>
        public void TestStorageGetAllStudies()
        {
            _testStudyStorageManager.Save(_testStudy);
            var testStudy2 = new Study();
            _testStudyStorageManager.Save(testStudy2);
            Assert.AreEqual(2, _testStudyStorageManager.GetAll().Count());
        }

        /// <summary>
        ///     Tests if a study has been removed from the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageRemoveStudy()
        {
            _testStudyStorageManager.Save(_testStudy);
            Assert.AreEqual(1, _studies.Values.ToList().Count);
            _testStudyStorageManager.Remove(_testStudy.ID);
            Assert.AreEqual(0, _studies.Values.ToList().Count);
        }

        /// <summary>
        ///     Tests if an exception is thrown and Remove() returns false if one tries to remove a study, while
        ///     there are no studies to remove
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void TestStorageNoStudyToRemove()
        {
            Assert.AreEqual(0, _studies.Values.ToList().Count);
            Assert.IsFalse(_testStudyStorageManager.Remove(_testStudy.ID));
        }

        /// <summary>
        ///     Tests if a study is updated
        /// </summary>
        [TestMethod]
        public void TestStorageUpdateStudy()
        {
            var stageTestStudy = new Study {ID = 1, Name = "study"};
            _testStudyStorageManager.Save(stageTestStudy);
            stageTestStudy.Name = "updatedname";
            _testStudyStorageManager.Update(stageTestStudy);
            Assert.AreEqual("updatedname", _testStudyStorageManager.Get(1).Name);
        }
    }
}