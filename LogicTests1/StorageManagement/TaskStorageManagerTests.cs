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
    public class TaskStorageManagerTests
    {
        // Dictionary<int, StoredTask> _tasks;
        Mock<IRepository> mockTaskRepo;
        int id = 1;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            // _tasks = new Dictionary<int, StoredTask>();
            mockTaskRepo = new Mock<IRepository>();

            // Read item
            // Read items
            // Create 
            // Update
            // Delete

        }

        /// <summary>
        /// Tests if a task has been added to the mock repo
        /// </summary>

        [TestMethod()]
        public void AddTaskTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests if a task has been removed to the mock repo
        /// </summary>

        [TestMethod()]
        public void RemoveTaskTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests if an exception is thrown if one tries to remove a task, while
        /// there are no tasks to remove
        /// </summary>

        [TestMethod()]
        public void NoTaskToRemoveTest()
        {
            throw new NotImplementedException();
        }
    }
}