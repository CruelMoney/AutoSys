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
    public class TeamStorageManagerTests
    {
        // Dictionary<int, StoredTeam> _teams;
        Mock<IGenericRepository> mockTeamRepo;
        int id = 1;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            // _teams = new Dictionary<int, StoredTeam>();
            mockTeamRepo = new Mock<IGenericRepository>();

            // Read item
            // Read items
            // Create 
            // Update
            // Delete

        }

        /// <summary>
        /// Tests if a team has been added to the mock repo
        /// </summary>

        [TestMethod()]
        public void AddTeamTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests if a team has been removed to the mock repo
        /// </summary>

        [TestMethod()]
        public void RemoveTeamTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests if an exception is thrown if one tries to remove a team, while
        /// there are no teams to remove
        /// </summary>

        [TestMethod()]
        public void NoTeamToRemoveTest()
        {
            throw new NotImplementedException();
        }
    }
}