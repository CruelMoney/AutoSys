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
    public class TaskStorageManagerTests
    {
        private int _id;
        private Mock<IGenericRepository> _mockTaskRepo;
        private Dictionary<int, StudyTask> _tasks;
        private readonly StudyTask _testTask = new StudyTask {ID = 1};
        private TaskStorageManager _testTaskStorageManager;

        [TestInitialize]
        public void InitializeRepo()
        {
            _id = 1;
            _tasks = new Dictionary<int, StudyTask>();
            _mockTaskRepo = new Mock<IGenericRepository>();
            _testTaskStorageManager = new TaskStorageManager(_mockTaskRepo.Object);

            // Read item - StudyTask
            _mockTaskRepo.Setup(r => r.Read<StudyTask>(It.IsAny<int>()))
                .Returns<int>(id => _tasks.First(e => e.Key == id).Value);

            // Read items - StudyTask
            _mockTaskRepo.Setup(r => r.Read<StudyTask>()).Returns(_tasks.Values.AsQueryable());

            // Create - StudyTask
            _mockTaskRepo.Setup(r => r.Create(It.IsAny<StudyTask>())).Callback<StudyTask>(task =>
            {
                var nextId = _id++;
                task.ID = nextId;
                _tasks.Add(nextId, task);
            });

            // Update - StudyTask
            _mockTaskRepo.Setup(r => r.Update(It.IsAny<StudyTask>())).Callback<StudyTask>(task =>
            {
                if (_tasks.ContainsKey(task.ID))
                {
                    _tasks[task.ID] = task;
                }
            });

            // Delete - StudyTask
            _mockTaskRepo.Setup(r => r.Delete(It.IsAny<StudyTask>()))
                .Callback<StudyTask>(task => { _tasks.Remove(task.ID); });
        }

        /// <summary>
        ///     Tests if a StudyTask has been added to the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageAddTask()
        {
            _testTaskStorageManager.CreateTask(_testTask);
            Assert.AreEqual(1, _tasks.Values.ToList().Count);
        }

        /// <summary>
        ///     Tests get on a Study Task from the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageGetTask()
        {
            //testTaskStorageManager.CreateTask(testTask);
            //Assert.AreEqual(1, testTaskStorageManager.GetTask(1));
        }

        /// <summary>
        ///     Tests get on all tasks in the mock repo
        /// </summary>
        public void TestStorageGetAllTasks()
        {
            _testTaskStorageManager.CreateTask(_testTask);
            var testTask2 = new StudyTask();
            _testTaskStorageManager.CreateTask(testTask2);
            Assert.AreEqual(2, _testTaskStorageManager.GetAllTasks().Count());
        }

        /// <summary>
        ///     Tests if a StudyTask has been removed to the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageRemoveTask()
        {
            _testTaskStorageManager.CreateTask(_testTask);
            Assert.AreEqual(1, _tasks.Values.ToList().Count);
            _testTaskStorageManager.RemoveTask(1);
            Assert.AreEqual(0, _tasks.Values.ToList().Count);
        }

        /// <summary>
        ///     Tests if an exception is thrown and Remove() returns false if one tries to remove a StudyTask, while
        ///     there are no tasks to remove
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void TestStorageNoTaskToRemove()
        {
            Assert.AreEqual(0, _tasks.Values.ToList().Count);
            Assert.IsFalse(_testTaskStorageManager.RemoveTask(1));
        }

        /// <summary>
        ///     Tests if a task is updated
        /// </summary>
        // Giver ikke rigtig mening at have
        [TestMethod]
        public void TestStorageUpdateTask()
        {
            var testTask = new StudyTask {ID = 1, TaskType = StudyTask.Type.Review};
            _testTaskStorageManager.CreateTask(testTask);
            testTask.TaskType = StudyTask.Type.Conflict;
            _testTaskStorageManager.UpdateTask(testTask);
            Assert.AreEqual(StudyTask.Type.Conflict, _testTaskStorageManager.GetTask(1).TaskType);
        }
    }
}