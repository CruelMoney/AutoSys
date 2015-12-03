﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage.Repository;

namespace LogicTests1.StorageManagement
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

            // Read item - Team
            mockTaskRepo.Setup(r => r.Read<StudyTask>(It.IsAny<int>())).Returns<int>((id) => _tasks.First(e => e.Key == id).Value);

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
            testTaskStorageManager.RemoveTask(1);
            Assert.AreEqual(0, _tasks.Values.ToList().Count);
        }

        /// <summary>
        /// Tests if an exception is thrown if one tries to remove a StudyTask, while
        /// there are no tasks to remove
        /// </summary>

        [TestMethod()]
        public void StorageNoTaskToRemoveTest()
        {
            Assert.AreEqual(0, _tasks.Values.ToList().Count);
            Assert.IsFalse(testTaskStorageManager.RemoveTask(1));
        }
    }
}