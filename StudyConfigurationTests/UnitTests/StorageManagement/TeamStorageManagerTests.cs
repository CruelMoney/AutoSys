#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.StorageManagement
{
    [TestClass]
    public class TeamStorageManagerTests
    {
        private int _id;
        private Mock<IGenericRepository> _mockTeamRepo;
        private Dictionary<int, Team> _teams;
        private Team _testTeam;
        private TeamStorageManager _testTeamStorageManager;

        [TestInitialize]
        public void InitializeRepo()
        {
            _id = 1;
            _teams = new Dictionary<int, Team>();
            _mockTeamRepo = new Mock<IGenericRepository>();
            _testTeam = new Team {ID = 1};
            _testTeamStorageManager = new TeamStorageManager(_mockTeamRepo.Object);

            // Read item - Team
            _mockTeamRepo.Setup(r => r.Read<Team>(It.IsAny<int>()))
                .Returns<int>(id => _teams.First(e => e.Key == id).Value);

            // Read items - Team
            _mockTeamRepo.Setup(r => r.Read<Team>()).Returns(_teams.Values.AsQueryable());

            // Create - Team
            _mockTeamRepo.Setup(r => r.Create(It.IsAny<Team>())).Callback<Team>(team =>
            {
                var nextId = _id++;
                team.ID = nextId;
                _teams.Add(nextId, team);
            });

            // Update - Team
            _mockTeamRepo.Setup(r => r.Update(It.IsAny<Team>())).Callback<Team>(team =>
            {
                if (_teams.ContainsKey(team.ID))
                {
                    _teams[team.ID] = team;
                }
            });

            // Delete - Team
            _mockTeamRepo.Setup(r => r.Delete(It.IsAny<Team>())).Callback<Team>(team => { _teams.Remove(team.ID); });
        }

        /// <summary>
        ///     Tests if a team has been added to the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageAddTeam()
        {
            _testTeamStorageManager.CreateTeam(_testTeam);
            Assert.AreEqual(1, _teams.Values.ToList().Count);
        }

        /// <summary>
        ///     Tests get on a team from the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageGetTeam()
        {
            _testTeamStorageManager.CreateTeam(_testTeam);
            Assert.AreEqual(_testTeam, _testTeamStorageManager.GetTeam(1));
            Assert.AreEqual(1, _testTeamStorageManager.GetTeam(1).ID);
        }

        /// <summary>
        ///     Tests get on all teams in the mock repo
        /// </summary>
        public void TestStorageGetAllTeams()
        {
            _testTeamStorageManager.CreateTeam(_testTeam);
            var testTeam2 = new Team();
            _testTeamStorageManager.CreateTeam(testTeam2);
            Assert.AreEqual(2, _testTeamStorageManager.GetAllTeams().Count());
        }

        /// <summary>
        ///     Tests if a team has been removed to the mock repo
        /// </summary>
        [TestMethod]
        public void TestStorageRemoveTeam()
        {
            _testTeamStorageManager.CreateTeam(_testTeam);
            Assert.AreEqual(1, _teams.Values.ToList().Count);
            _testTeamStorageManager.RemoveTeam(1);
            Assert.AreEqual(0, _teams.Values.ToList().Count);
        }

        /// <summary>
        ///     Tests if an exception is thrown and Remove() returns false if one tries to remove a team, while
        ///     there are no teams to remove
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof (InvalidOperationException))]
        public void TestStorageNoTeamToRemove()
        {
            Assert.AreEqual(0, _teams.Values.ToList().Count);
            Assert.IsFalse(_testTeamStorageManager.RemoveTeam(1));
        }

        /// <summary>
        ///     Tests if a team is updated
        /// </summary>
        [TestMethod]
        public void TestStorageUpdateTeam()
        {
            var testTeam = new Team {ID = 1, Name = "Team"};
            _testTeamStorageManager.CreateTeam(testTeam);
            testTeam.Name = "Team Awesome";
            _testTeamStorageManager.UpdateTeam(testTeam);
            Assert.AreEqual("Team Awesome", _testTeamStorageManager.GetTeam(1).Name);
        }
    }
}