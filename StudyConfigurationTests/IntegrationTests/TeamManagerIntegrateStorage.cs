#region Using

using System;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.TeamUserManagement;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServerTests.IntegrationTests.DBInitializers;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests
{
    [TestClass]
    public class TeamManagerIntegrationTest
    {
        private readonly TeamDto _teamDto = new TeamDto { Name = "Team", UserIDs = new []{1,2,3}};
        private TeamManager _teamManager;

        [TestInitialize]
        public void InitializeTest()
        {
            //Create Empty DB
            Database.SetInitializer(new MultipleTeamsDb());
            var context = new StudyContext();
            context.Database.Initialize(true);

            _teamManager = new TeamManager();
        }

        [ExpectedException(typeof(ArgumentException))]
        [TestMethod]
        public void TestIntegrationTeamManagerCreateTeamNoUsers()
        {
            var team = new TeamDto() { Name = "team"};

            _teamManager.CreateTeam(team);
        }

        [ExpectedException(typeof(NullReferenceException))]
        [TestMethod]
        public void TestIntegrationTeamManagerCreateTeamUserNotInDB()
        {
            var team = new TeamDto() { Name = "team", UserIDs = new []{100}};

            _teamManager.CreateTeam(team);
        }

        [TestMethod]
        public void TestIntegrationTeamManagerCreateTeam()
        {
            _teamManager.CreateTeam(_teamDto);
            Assert.AreEqual("Team", _teamManager.GetTeamDto(4).Name);
        }

        [TestMethod]
        public void TestIntegrationTeamManagerRemoveTeam()
        {
            _teamManager.CreateTeam(_teamDto);

            Assert.IsTrue(_teamManager.RemoveTeam(4));
        }

        [TestMethod]
        public void TestIntegrationTeamManagerUpdateTeam()
        {
            _teamManager.CreateTeam(_teamDto);
            _teamDto.Name = "Team Awesome";
            Assert.IsTrue(_teamManager.UpdateTeam(4, _teamDto));
            Assert.AreEqual("Team Awesome", _teamManager.GetTeamDto(4).Name);
        }

        [TestMethod]
        public void TestIntegrationTeamManagerSearchTeams()
        {
            _teamManager.CreateTeam(_teamDto);
            Assert.AreEqual(1, _teamManager.SearchTeamDtOs("Team").Count());
        }

        [TestMethod]
        public void TestIntegrationTeamManagerGetAllTeams()
        {
            Assert.AreEqual(3, _teamManager.GetAllTeamDtOs().Count());
        }
    }
}