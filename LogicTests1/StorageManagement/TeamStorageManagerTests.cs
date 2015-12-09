using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;

namespace LogicTests1.StorageManagement
{
    [TestClass]
    public class TeamStorageManagerTests
    {
        Dictionary<int, Team> _teams;
        Mock<IGenericRepository> mockTeamRepo;
        int id;
        Team testTeam;
        TeamStorageManager testTeamStorageManager;

        [TestInitialize]
        public void InitializeRepo()
        {
            id = 1;
            _teams = new Dictionary<int, Team>();
            mockTeamRepo = new Mock<IGenericRepository>();
            testTeam = new Team() { Id = 1 };
            testTeamStorageManager = new TeamStorageManager(mockTeamRepo.Object);

            // Read item - Team
            mockTeamRepo.Setup(r => r.Read<Team>(It.IsAny<int>())).Returns<int>((id) => _teams.First(e => e.Key == id).Value);
            
            // Read items - Team
            mockTeamRepo.Setup(r => r.Read<Team>()).Returns(_teams.Values.AsQueryable());
            
            // Create - Team
            mockTeamRepo.Setup(r => r.Create<Team>(It.IsAny<Team>())).Callback<Team>(team =>
            {
                int nextId = id++;
                team.Id = nextId;
                _teams.Add(nextId, team);

            });
            
            // Update - Team
            mockTeamRepo.Setup(r => r.Update<Team>(It.IsAny<Team>())).Callback<Team>(team =>
            {
                if (_teams.ContainsKey(team.Id))
                {
                    _teams[team.Id] = team;
                }
            });
            
            // Delete - Team
            mockTeamRepo.Setup(r => r.Delete<Team>(It.IsAny<Team>())).Callback<Team>(team =>
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
            testTeamStorageManager.SaveTeam(testTeam);
            Assert.AreEqual(1, _teams.Values.ToList().Count);
        }

        /// <summary>
        /// Tests get on a team from the mock repo
        /// </summary>

        [TestMethod]
        public void StorageGetTeamTest() {
            testTeamStorageManager.SaveTeam(testTeam);
            Assert.AreEqual(testTeam, testTeamStorageManager.GetTeam(1));
            Assert.AreEqual(1, testTeamStorageManager.GetTeam(1).Id);
        }

        /// <summary>
        /// Tests get on all teams in the mock repo
        /// </summary>

        public void StorageGetAllTeamsTest()
        {
            testTeamStorageManager.SaveTeam(testTeam);
            var testTeam2 = new Team();
            testTeamStorageManager.SaveTeam(testTeam2);
            Assert.AreEqual(2, testTeamStorageManager.GetAllTeams().Count());
        }

        /// <summary>
        /// Tests if a team has been removed to the mock repo
        /// </summary>

        [TestMethod]
        public void StorageRemoveTeamTest()
        {
            testTeamStorageManager.SaveTeam(testTeam);
            Assert.AreEqual(1, _teams.Values.ToList().Count);
            testTeamStorageManager.RemoveTeam(1);
            Assert.AreEqual(0, _teams.Values.ToList().Count);
        }

        /// <summary>
        /// Tests if an exception is thrown and Remove() returns false if one tries to remove a team, while
        /// there are no teams to remove
        /// </summary>

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void StorageNoTeamToRemoveTest()
        {
            Assert.AreEqual(0, _teams.Values.ToList().Count);
            Assert.IsFalse(testTeamStorageManager.RemoveTeam(1));
        }

        /// <summary>
        /// Tests if a team is updated
        /// </summary>

        [TestMethod]
        public void StorageUpdateTeamTest()
        {
            var testTeam = new Team() { Id = 1, Name = "Team" };
            testTeamStorageManager.SaveTeam(testTeam);
            testTeam.Name = "Team Awesome";
            testTeamStorageManager.UpdateTeam(testTeam);
            Assert.AreEqual("Team Awesome", testTeamStorageManager.GetTeam(1).Name);
        }
    }
}