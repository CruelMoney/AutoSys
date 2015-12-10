using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Logic.TeamCRUD;
using StudyConfigurationServer.Logic.StorageManagement;
using System.Data.Entity;
using StudyConfigurationServer.Models.Data;
using Storage.Repository;
using StudyConfigurationServer.Api;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;
using System.Linq;

namespace LogicTests1.IntegrationTests
{
    [TestClass]
    public class TeamManagerIntegrationTests
    {
        private TeamManager teamManager;
        private TeamStorageManager teamStorageManager;
        private UserStorageManager userStorageManager;

        private TeamDTO teamDTO = new TeamDTO() { Id = 1, Name = "Team" };
        private IGenericRepository testRepo;

        [TestInitialize]
        public void InitializeTest()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<IntegrationTestContext>());
            IntegrationTestContext testContext = new IntegrationTestContext();
            testContext.Database.Initialize(true);

            testRepo = new EntityFrameworkGenericRepository<IntegrationTestContext>(testContext);
            userStorageManager = new UserStorageManager(testRepo);
            teamStorageManager = new TeamStorageManager(testRepo);
            teamManager = new TeamManager(teamStorageManager, userStorageManager);

        }

        [TestMethod]
        public void TestTeamManagerIntegrationCreateTeam()
        {

            teamManager.CreateTeam(teamDTO);
            Assert.AreEqual("Team", teamManager.GetTeam(teamDTO.Id).Name);
        }

        [TestMethod]
        public void TestTeamManagerIntegrationRemoveTeam()
        {

            teamManager.CreateTeam(teamDTO);

            Assert.IsTrue(teamManager.RemoveTeam(teamDTO.Id));
        }

        [TestMethod]
        public void TestTeamManagerIntegrationUpdateTeam()
        {
            teamManager.CreateTeam(teamDTO);
            teamDTO.Name = "Team2";
            Assert.IsTrue(teamManager.UpdateTeam(teamDTO.Id, teamDTO));
            Assert.AreEqual("Team2", teamManager.GetTeam(teamDTO.Id).Name);
        }

        [TestMethod]
        public void TestTeamManagerIntegrationSearchTeams()
        {
            teamManager.CreateTeam(teamDTO);
            Assert.AreEqual(1, teamManager.SearchTeams("Team").Count());
        }

        [TestMethod]
        public void TestTeamManagerIntegrationGetAllTeams()
        {
            TeamDTO teamDTO2 = new TeamDTO() { Id = 2, Name = "Team2" };
            teamManager.CreateTeam(teamDTO);
            teamManager.CreateTeam(teamDTO2);
            Assert.AreEqual(2, teamManager.GetAllTeams().Count());
        }
    }
}