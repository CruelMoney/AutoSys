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
        Dictionary<int, UserLogic> _users;
        Mock<IGenericRepository> mockUserRepo;
        int id;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            _users = new Dictionary<int, UserLogic>();
            mockUserRepo = new Mock<IGenericRepository>();

            // Read item - UserLogic
            mockUserRepo.Setup(r => r.Read<UserLogic>(It.IsAny<int>())).Returns<int, UserLogic>((id, user) => _users.First(e => e.Key == id).Value);

            // Read items - UserLogic
            mockUserRepo.Setup(r => r.Read<UserLogic>()).Returns(_users.Values.AsQueryable());

            // Create - UserLogic
            mockUserRepo.Setup(r => r.Create<UserLogic>(It.IsAny<UserLogic>())).Callback<UserLogic>(user =>
            {
                int nextId = id++;
                user.Id = nextId;
                _users.Add(nextId, user);

            });

            // Update - UserLogic
            mockUserRepo.Setup(r => r.Update<UserLogic>(It.IsAny<UserLogic>())).Callback<UserLogic>(user =>
            {
                if (_users.ContainsKey(user.Id))
                {
                    _users[user.Id] = user;
                }
            });

            // Delete - UserLogic
            mockUserRepo.Setup(r => r.Delete<UserLogic>(It.IsAny<UserLogic>())).Callback<UserLogic>(user =>
            {
                _users.Remove(user.Id);
            });

        }

        /// <summary>
        /// Tests if a user has been added to the mock repo
        /// </summary>

        [TestMethod]
        public void StorageSaveUserTest()
        {
            UserStorageManager testUserStorageManager = new UserStorageManager(mockUserRepo.Object);
            Assert.AreEqual(0, _users.Values.ToList().Count);
            var testUser = new UserLogic();
            testUserStorageManager.SaveUser(testUser);
            Assert.AreEqual(1, _users.Values.ToList().Count);
        }

        /// <summary>
        /// Tests if a user has been removed to the mock repo
        /// </summary>

        [TestMethod]
        public void StorageRemoveUserTest()
        {
            UserStorageManager testUserStorageManager = new UserStorageManager(mockUserRepo.Object);
            Assert.AreEqual(0, _users.Values.ToList().Count);
            var testUser = new UserLogic();
            testUserStorageManager.SaveUser(testUser);
            Assert.AreEqual(1, _users.Values.ToList().Count);
            _users.Remove(1);
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
            UserStorageManager testUserStorageManager = new UserStorageManager(mockUserRepo.Object);
            Assert.AreEqual(0, _users.Values.ToList().Count);
            _users.Remove(1);
        }
    }
}