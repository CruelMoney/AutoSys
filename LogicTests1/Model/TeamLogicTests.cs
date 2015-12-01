using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model.Tests
{
    [TestClass()]
    public class TeamLogicTests
    {
        TeamLogic testTeam;
        List<UserLogic> users = new List<UserLogic>();

        [TestInitialize]
        public void TeamInitialize() {
            testTeam = new TeamLogic();
            var user = new UserLogic();
            var user2 = new UserLogic();
            users.Add(user);
            users.Add(user2);
            testTeam.Id = 1;
            testTeam.Name = "testName";
            testTeam.Metadata = "testData";
            testTeam.Users = users;
        }

        [TestMethod()]
        public void CorrectTeamCreationTest()
        {
            Assert.AreEqual(1, testTeam.Id);
            Assert.AreEqual("testName", testTeam.Name);
            Assert.AreEqual("testData", testTeam.Metadata);
            Assert.AreEqual(2, users.Count);
        }
    }
}