using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.TeamCRUD;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;

namespace LogicTests1.StorageManagement
{
    [TestClass]
    public class UserStorageManagerTests
    {
        Dictionary<int, User> _users;
        Mock<IGenericRepository> mockUserRepo;
        int id;
        User testUser;
        UserStorageManager testUserStorageManager;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            _users = new Dictionary<int, User>();
            mockUserRepo = new Mock<IGenericRepository>();
            testUser = new User() { Id = 1 };
            testUserStorageManager = new UserStorageManager(mockUserRepo.Object);

            // Read item - User
            mockUserRepo.Setup(r => r.Read<User>(It.IsAny<int>())).Returns<int>((id) => _users.First(e => e.Key == id).Value);

            // Read items - User
            mockUserRepo.Setup(r => r.Read<User>()).Returns(_users.Values.AsQueryable());

            // Create - User
            mockUserRepo.Setup(r => r.Create<User>(It.IsAny<User>())).Callback<User>(user =>
            {
                int nextId = id++;
                user.Id = nextId;
                _users.Add(nextId, user);

            });

            // Update - User
            mockUserRepo.Setup(r => r.Update<User>(It.IsAny<User>())).Callback<User>(user =>
            {
                if (_users.ContainsKey(user.Id))
                {
                    _users[user.Id] = user;
                }
            });

            // Delete - User
            mockUserRepo.Setup(r => r.Delete<User>(It.IsAny<User>())).Callback<User>(user =>
            {
                _users.Remove(user.Id);
            });

        }

        /// <summary>
        /// Tests if a User has been added to the mock repo
        /// </summary>

        [TestMethod]
        public void StorageAddUserTest()
        {
            testUserStorageManager.SaveUser(testUser);
            Assert.AreEqual(1, _users.Values.ToList().Count);
        }

        /// <summary>
        /// Tests get on a User from the mock repo
        /// </summary>

        [TestMethod]
        public void StorageGetUserTest()
        {
            testUserStorageManager.SaveUser(testUser);
            Assert.AreEqual(testUser, testUserStorageManager.GetUser(1));
            Assert.AreEqual(1, testUserStorageManager.GetUser(1).Id);
        }

        /// <summary>
        /// Tests if a User has been removed to the mock repo
        /// </summary>

        [TestMethod]
        public void StorageRemoveUserTest()
        {
            testUserStorageManager.SaveUser(testUser);
            Assert.AreEqual(1, _users.Values.ToList().Count);
            testUserStorageManager.RemoveUser(1);
            Assert.AreEqual(0, _users.Values.ToList().Count);
        }

        /// <summary>
        /// Tests if an exception is thrown and Remove() returns false if one tries to remove a User, while
        /// there are no users to remove
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StorageNoUserToRemoveTest()
        {
            Assert.AreEqual(0, _users.Values.ToList().Count);
            Assert.IsFalse(testUserStorageManager.RemoveUser(1));
        }

        /// <summary>
        /// Tests if a User is updated
        /// </summary>
        
        [TestMethod]
        public void StorageUpdateUserTest()
        {
            var testUser1 = new User() { Id = 1, Name = "Bob" };
            testUserStorageManager.SaveUser(testUser1);
            var testUser2 = new User() { Id = 1, Name = "Bob Sveskebob" };
            testUserStorageManager.UpdateUser(testUser2);
            Assert.AreEqual("Bob Sveskebob", testUserStorageManager.GetUser(1).Name);
        }

      
       
    }
}