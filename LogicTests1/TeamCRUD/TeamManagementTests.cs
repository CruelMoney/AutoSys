using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Models;

namespace LogicTests1.TeamCRUD
{
    [TestClass()]
    public class TeamManagementTests
    {
        Team testTeam;
        User testUser1;
        User testUser2;
        List<User> _users = new List<User>();

        [TestInitialize]
        public void InitializeTeamTests() {
            testUser1 = new User() { Id = 1, Name = "testUser1" };
            testUser2 = new User() { Id = 2, Name = "testUser2" };
            _users.Add(testUser1);
            testTeam = new Team() { Id = 1, Name = "Test Team", Users = _users };
        }

        [TestMethod()]
        public void TestTeamCreate()
        {
            Assert.AreEqual("Test Team", testTeam.Name);
        }

        [TestMethod()]
        public void TestTeamAddUser()
        {
            testTeam.Users.Add(testUser2);
            Assert.AreEqual(2, testTeam.Users.Count);
        }

        [TestMethod()]
        public void TestTeamRemoveUser()
        {
            testTeam.Users.Remove(testUser1);
            Assert.AreEqual(0, testTeam.Users.Count);
        }
    }
}