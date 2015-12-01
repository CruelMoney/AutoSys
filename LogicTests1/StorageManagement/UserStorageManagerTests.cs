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
        /// Tests if a user has been added to the mock repo
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
        /// Tests if a user has been removed to the mock repo
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
        /// Tests if an exception is thrown if one tries to remove a user, while
        /// there are no users to remove
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StorageNoUserToRemoveTest()
        {
            Assert.AreEqual(0, _users.Values.ToList().Count);
            Assert.IsFalse(testUserStorageManager.RemoveUser(1));
        }
    }
}