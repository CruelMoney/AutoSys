using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.StorageManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Storage.Repository;

namespace Logic.StorageManagement.Tests
{
    [TestClass()]
    public class StudyStorageManagerTests
    {
        // Dictionary<int, StoredStudy> _studies;
        Mock<IRepository> mockStudyRepo;
        int id = 1;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            // _studies = new Dictionary<int, StoredStudy>();
            mockStudyRepo = new Mock<IRepository>();

            // Read item
            // Read items
            // Create 
            // Update
            // Delete

        }

        /// <summary>
        /// Tests if a study has been added to the mock repo
        /// </summary>

        [TestMethod()]
        public void AddStudyTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests if a study has been removed to the mock repo
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