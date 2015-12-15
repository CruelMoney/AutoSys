#region Using

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.TeamCRUD
{
    [TestClass]
    public class TeamManagementTests
    {
        private Team _testTeam;
        private User _testUser1;
        private User _testUser2;
        private readonly List<User> _users = new List<User>();

        [TestInitialize]
        public void InitializeTeamTests()
        {
            _testUser1 = new User {ID = 1, Name = "testUser1"};
            _testUser2 = new User {ID = 2, Name = "testUser2"};
            _users.Add(_testUser1);
            _testTeam = new Team {ID = 1, Name = "Test Team", Users = _users};
        }

        [TestMethod]
        public void TestTeamCreate()
        {
            Assert.AreEqual("Test Team", _testTeam.Name);
        }

        [TestMethod]
        public void TestTeamAddUser()
        {
            _testTeam.Users.Add(_testUser2);
            Assert.AreEqual(2, _testTeam.Users.Count);
        }

        [TestMethod]
        public void TestTeamRemoveUser()
        {
            _testTeam.Users.Remove(_testUser1);
            Assert.AreEqual(0, _testTeam.Users.Count);
        }
    }
}