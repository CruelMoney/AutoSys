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

namespace Logic.StorageManagement.Tests
{
    [TestClass]
    public class TaskStorageManagerTests
    {
        Dictionary<int, TaskLogic> _tasks;
        Mock<IGenericRepository> mockTaskRepo;
        int id;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            _tasks = new Dictionary<int, TaskLogic>();
            mockTaskRepo = new Mock<IGenericRepository>();

            // Read items - TaskLogic
            mockTaskRepo.Setup(r => r.Read<TaskLogic>()).Returns(_tasks.Values.AsQueryable());

            // Create - TaskLogic
            mockTaskRepo.Setup(r => r.Create<TaskLogic>(It.IsAny<TaskLogic>())).Callback<TaskLogic>(task =>
            {
                int nextId = id++;
                task.Id = nextId;
                _tasks.Add(nextId, task);

            });

            // Update - TaskLogic
            mockTaskRepo.Setup(r => r.Update<TaskLogic>(It.IsAny<TaskLogic>())).Callback<TaskLogic>(task =>
            {
                if (_tasks.ContainsKey(task.Id))
                {
                    _tasks[task.Id] = task;
                }
            });

            // Delete - TaskLogic
            mockTaskRepo.Setup(r => r.Delete<TaskLogic>(It.IsAny<TaskLogic>())).Callback<TaskLogic>(task =>
            {
                _tasks.Remove(task.Id);
            });

        }

        /// <summary>
        /// Tests if a task has been added to the mock repo
        /// </summary>

        [TestMethod]
        public void StorageAddTaskTest()
        {
            TaskStorageManager testTaskStorageManager = new TaskStorageManager(mockTaskRepo.Object);
            Assert.AreEqual(0, _tasks.Values.ToList().Count);
            var testTask = new TaskLogic();
            testTaskStorageManager.CreateTask(testTask);
            Assert.AreEqual(1, _tasks.Values.ToList().Count);
        }

        /// <summary>
        /// Tests if a task has been removed to the mock repo
        /// </summary>

        [TestMethod()]
        public void StorageRemoveTaskTest()
        {
            TaskStorageManager testTaskStorageManager = new TaskStorageManager(mockTaskRepo.Object);
            Assert.AreEqual(0, _tasks.Values.ToList().Count);
            var testTask = new TaskLogic();
            testTaskStorageManager.CreateTask(testTask);
            Assert.AreEqual(1, _tasks.Values.ToList().Count);
            _tasks.Remove(1);
            Assert.AreEqual(0, _tasks.Values.ToList().Count);
        }

        /// <summary>
        /// Tests if an exception is thrown if one tries to remove a task, while
        /// there are no tasks to remove
        /// </summary>

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StorageNoTaskToRemoveTest()
        {
            TaskStorageManager testTaskStorageManager = new TaskStorageManager(mockTaskRepo.Object);
            Assert.AreEqual(0, _tasks.Values.ToList().Count);
            _tasks.Remove(1);
        }
    }
}