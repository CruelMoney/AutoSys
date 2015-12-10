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
    public class TeamManagerIntegrationTest
    {
        private TeamManager teamManager;
        private TeamStorageManager teamStorageManager;

        private TeamDTO teamDTO = new TeamDTO() { Id = 1, Name = "Team", Metadata = "Metadata" ,UserIDs = new int[] { 1, 2, 3 } };
        private IGenericRepository testRepo;
        private User user1 = new User() { Id = 1, Name = "user1" };
        private User user2 = new User() { Id = 2, Name = "user2" };
        private User user3 = new User() { Id = 3, Name = "user3" };

        [TestInitialize]
        public void InitializeTest()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<IntegrationTestContext>());
            IntegrationTestContext testContext = new IntegrationTestContext();
            testContext.Database.Initialize(true);

            testRepo = new EntityFrameworkGenericRepository<IntegrationTestContext>(testContext);
            teamStorageManager = new TeamStorageManager(testRepo);
            teamStorageManager.SaveUser(user1);
            teamStorageManager.SaveUser(user2);
            teamStorageManager.SaveUser(user3);
            teamManager = new TeamManager(teamStorageManager);
            
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
        public void TestTeamManagerIntegrationSearchTeam()
        {
            teamManager.CreateTeam(teamDTO);
            Assert.AreEqual(1, teamManager.SearchTeams("Team").Count());
        }

        [TestMethod]
        public void TestTeamManagerIntegrationGetAllTeams()
        {
            TeamDTO teamDTO2 = new TeamDTO() { Id = 2, Name = "Team2", Metadata = "metadata", UserIDs = new int[] { 1, 2 } };
            teamManager.CreateTeam(teamDTO);
            teamManager.CreateTeam(teamDTO2);
            Assert.AreEqual(2, teamManager.GetAllTeams().Count());
        }
    }
}