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
    public class UserStorageManagerTests
    {
        private int _id;
        private Mock<IGenericRepository> _mockUserRepo;
        private User _testUser;
        private TeamStorageManager _testUserStorageManager;
        private Dictionary<int, User> _users;

        [TestInitialize]
        public void InitializeRepo()
        {
            _id = 1;
            _users = new Dictionary<int, User>();
            _mockUserRepo = new Mock<IGenericRepository>();
            _testUser = new User {ID = 1};
            _testUserStorageManager = new TeamStorageManager(_mockUserRepo.Object);

            // Read item - User
            _mockUserRepo.Setup(r => r.Read<User>(It.IsAny<int>()))
                .Returns<int>(id => _users.First(e => e.Key == id).Value);

            // Read items - User
            _mockUserRepo.Setup(r => r.Read<User>()).Returns(_users.Values.AsQueryable());

            // Create - User
            _mockUserRepo.Setup(r => r.Create(It.IsAny<User>())).Callback<User>(user =>
            {
                var nextId = _id++;
                user.ID = nextId;
                _users.Add(nextId, user);
            });

            // Update - User
            _mockUserRepo.Setup(r => r.Update(It.IsAny<User>())).Callback<User>(user =>
            {
                if (_users.ContainsKey(user.ID))
                {
                    _users[user.ID] = user;
                }
            });

            // Delete - User
            _mockUserRepo.Setup(r => r.Delete(It.IsAny<User>())).Callback<User>(user => { _users.Remove(user.ID); });
        }

        /// <summary>
        ///     Tests if a User has been added to the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageAddUser()
        {
            _testUserStorageManager.SaveUser(_testUser);
            Assert.AreEqual(1, _users.Values.ToList().Count);
        }

        /// <summary>
        ///     Tests get on a User from the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageGetUser()
        {
            _testUserStorageManager.SaveUser(_testUser);
            Assert.AreEqual(_testUser, _testUserStorageManager.GetUser(1));
            Assert.AreEqual(1, _testUserStorageManager.GetUser(1).ID);
        }

        /// <summary>
        ///     Tests get on all users in the mock repo
        /// </summary>
        public void TestStorageGetAllUsers()
        {
            _testUserStorageManager.SaveUser(_testUser);
            var testUser2 = new User();
            _testUserStorageManager.SaveUser(testUser2);
            Assert.AreEqual(2, _testUserStorageManager.GetAllUsers().Count());
        }

        /// <summary>
        ///     Tests if a User has been removed to the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageRemoveUser()
        {
            _testUserStorageManager.SaveUser(_testUser);
            Assert.AreEqual(1, _users.Values.ToList().Count);
            _testUserStorageManager.RemoveUser(1);
            Assert.AreEqual(0, _users.Values.ToList().Count);
        }

        /// <summary>
        ///     Tests if an exception is thrown and Remove() returns false if one tries to remove a User, while
        ///     there are no users to remove
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void TestStorageNoUserToRemove()
        {
            Assert.AreEqual(0, _users.Values.ToList().Count);
            Assert.IsFalse(_testUserStorageManager.RemoveUser(1));
        }

        /// <summary>
        ///     Tests if a User is updated
        /// </summary>
        [TestMethod]
        public void TestStorageUpdateUser()
        {
            var testUser = new User {ID = 1, Name = "Bob"};
            _testUserStorageManager.SaveUser(testUser);
            testUser.Name = "Bob Sveskebob";
            _testUserStorageManager.UpdateUser(testUser);
            Assert.AreEqual("Bob Sveskebob", _testUserStorageManager.GetUser(1).Name);
        }
    }
}