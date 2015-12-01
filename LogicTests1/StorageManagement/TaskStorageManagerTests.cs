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
        Dictionary<int, StudyTask> _tasks;
        Mock<IGenericRepository> mockTaskRepo;
        int id;
        StudyTask testTask = new StudyTask() { Id = 1 };
        TaskStorageManager testTaskStorageManager;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            _tasks = new Dictionary<int, StudyTask>();
            mockTaskRepo = new Mock<IGenericRepository>();
            testTaskStorageManager = new TaskStorageManager(mockTaskRepo.Object);


            // Read items - StudyTask
            mockTaskRepo.Setup(r => r.Read<StudyTask>()).Returns(_tasks.Values.AsQueryable());

            // Create - StudyTask
            mockTaskRepo.Setup(r => r.Create<StudyTask>(It.IsAny<StudyTask>())).Callback<StudyTask>(task =>
            {
                int nextId = id++;
                task.Id = nextId;
                _tasks.Add(nextId, task);

            });

            // Update - StudyTask
            mockTaskRepo.Setup(r => r.Update<StudyTask>(It.IsAny<StudyTask>())).Callback<StudyTask>(task =>
            {
                if (_tasks.ContainsKey(task.Id))
                {
                    _tasks[task.Id] = task;
                }
            });

            // Delete - StudyTask
            mockTaskRepo.Setup(r => r.Delete<StudyTask>(It.IsAny<StudyTask>())).Callback<StudyTask>(task =>
            {
                _tasks.Remove(task.Id);
            });

        }

        /// <summary>
        /// Tests if a StudyTask has been added to the mock repo
        /// </summary>

        [TestMethod]
        public void StorageAddTaskTest()
        {
            testTaskStorageManager.CreateTask(testTask);
            Assert.AreEqual(1, _tasks.Values.ToList().Count);
        }

        /// <summary>
        /// Tests get on a Study Task from the mock repo
        /// </summary>

        [TestMethod]
        public void StorageGetTaskTest()
        {
            //testTaskStorageManager.CreateTask(testTask);
            //Assert.AreEqual(1, testTaskStorageManager.GetTask(1));
        }

        /// <summary>
        /// Tests if a StudyTask has been removed to the mock repo
        /// </summary>

        [TestMethod]
        public void StorageRemoveTaskTest()
        {
            testTaskStorageManager.CreateTask(testTask);
            Assert.AreEqual(1, _tasks.Values.ToList().Count);
            testTaskStorageManager.RemoveTask(testTask);
            Assert.AreEqual(0, _tasks.Values.ToList().Count);
        }

        /// <summary>
        /// Tests if an exception is thrown if one tries to remove a StudyTask, while
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