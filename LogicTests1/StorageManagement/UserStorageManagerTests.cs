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
    public class UserStorageManagerTests
    {
        // Dictionary<int, StoredUser> _users;
        Mock<IGenericRepository> mockUserRepo;
        int id = 1;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            // _users = new Dictionary<int, StoredUser>();
            mockUserRepo = new Mock<IGenericRepository>();

            // Read item
            // Read items
            // Create 
            // Update
            // Delete

        }

        /// <summary>
        /// Tests if a user has been added to the mock repo
        /// </summary>

        [TestMethod()]
        public void AddUserTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests if a user has been removed to the mock repo
        /// </summary>

        [TestMethod()]
        public void RemoveUserTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests if an exception is thrown if one tries to remove a user, while
        /// there are no users to remove
        /// </summary>

        [TestMethod()]
        public void NoUserToRemoveTest()
        {
            throw new NotImplementedException();
        }
    }
}