#region Using

using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.TeamCRUD;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests
{
    [TestClass]
    public class TeamManagerIntegrationTest
    {
        private readonly TeamDto _teamDto = new TeamDto {Id = 1, Name = "Team"};
        private TeamManager _teamManager;
        private TeamStorageManager _teamStorageManager;
        private IGenericRepository _testRepo;

        [TestInitialize]
        public void InitializeTest()
        {
            //Create Empty DB
            Database.SetInitializer(new DropCreateDatabaseAlways<IntegrationTestContext>());
            var testContext = new IntegrationTestContext();
            testContext.Database.Initialize(true);

            _testRepo = new EntityFrameworkGenericRepository<IntegrationTestContext>(testContext);
            _teamStorageManager = new TeamStorageManager(_testRepo);
            _teamManager = new TeamManager(_teamStorageManager);
        }

        [TestMethod]
        public void TestIntegrationTeamManagerCreateTeam()
        {
            _teamManager.CreateTeam(_teamDto);
            Assert.AreEqual("Team", _teamManager.GetTeam(_teamDto.Id).Name);
        }

        [TestMethod]
        public void TestIntegrationTeamManagerRemoveTeam()
        {
            _teamManager.CreateTeam(_teamDto);

            Assert.IsTrue(_teamManager.RemoveTeam(_teamDto.Id));
        }

        [TestMethod]
        public void TestIntegrationTeamManagerUpdateTeam()
        {
            _teamManager.CreateTeam(_teamDto);
            _teamDto.Name = "Team Awesome";
            Assert.IsTrue(_teamManager.UpdateTeam(_teamDto.Id, _teamDto));
            Assert.AreEqual("Team Awesome", _teamManager.GetTeam(_teamDto.Id).Name);
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
            var teamDto2 = new TeamDto {Id = 2, Name = "Team2"};

            _teamManager.CreateTeam(_teamDto);
            _teamManager.CreateTeam(teamDto2);

            Assert.AreEqual(2, _teamManager.GetAllTeamDtOs().Count());
        }
    }
}