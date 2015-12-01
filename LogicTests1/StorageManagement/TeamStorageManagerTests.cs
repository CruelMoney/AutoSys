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
    public class TeamStorageManagerTests
    {
        Dictionary<int, TeamLogic> _teams;
        Mock<IGenericRepository> mockTeamRepo;
        int id;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            _teams = new Dictionary<int, TeamLogic>();

            mockTeamRepo = new Mock<IGenericRepository>();
            
            // Read item - TeamLogic
            mockTeamRepo.Setup(r => r.Read<TeamLogic>(It.IsAny<int>())).Returns<int, TeamLogic>((id, team) => _teams.First(e => e.Key == id).Value);
            
            // Read items - TeamLogic
            mockTeamRepo.Setup(r => r.Read<TeamLogic>()).Returns(_teams.Values.AsQueryable());
            
            // Create - TeamLogic
            mockTeamRepo.Setup(r => r.Create<TeamLogic>(It.IsAny<TeamLogic>())).Callback<TeamLogic>(team =>
            {
                int nextId = id++;
                team.Id = nextId;
                _teams.Add(nextId, team);

            });
            
            // Update - TeamLogic
            mockTeamRepo.Setup(r => r.Update<TeamLogic>(It.IsAny<TeamLogic>())).Callback<TeamLogic>(team =>
            {
                if (_teams.ContainsKey(team.Id))
                {
                    _teams[team.Id] = team;
                }
            });
            
            // Delete - TeamLogic
            mockTeamRepo.Setup(r => r.Delete<TeamLogic>(It.IsAny<TeamLogic>())).Callback<TeamLogic>(team =>
            {
                _teams.Remove(team.Id);
            });

        }

        /// <summary>
        /// Tests if a team has been added to the mock repo
        /// </summary>

        [TestMethod]
        public void StorageAddTeamTest()
        {
            TeamStorageManager testTeamStorageManager = new TeamStorageManager(mockTeamRepo.Object);
            Assert.AreEqual(0, _teams.Values.ToList().Count);
            var testTeam = new TeamLogic();
            testTeamStorageManager.SaveTeam(testTeam);
            Assert.AreEqual(1, _teams.Values.ToList().Count);
        }

        /// <summary>
        /// Tests if a team has been removed to the mock repo
        /// </summary>

        [TestMethod]
        public void StorageRemoveTeamTest()
        {
            TeamStorageManager testTeamStorageManager = new TeamStorageManager(mockTeamRepo.Object);
            Assert.AreEqual(0, _teams.Values.ToList().Count);
            var testTeam = new TeamLogic();
            testTeamStorageManager.SaveTeam(testTeam);
            Assert.AreEqual(1, _teams.Values.ToList().Count);
            _teams.Remove(1);
            Assert.AreEqual(0, _teams.Values.ToList().Count);
        }

        /// <summary>
        /// Tests if an exception is thrown if one tries to remove a team, while
        /// there are no teams to remove
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StorageNoTeamToRemoveTest()
        {
            TeamStorageManager testTeamStorageManager = new TeamStorageManager(mockTeamRepo.Object);
            Assert.AreEqual(0, _teams.Values.ToList().Count);
            _teams.Remove(1);
        }
    }
}