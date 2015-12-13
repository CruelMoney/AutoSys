﻿using System;
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
            testTeam = new Team() { ID = 1 };
            testTeamStorageManager = new TeamStorageManager(mockTeamRepo.Object);

            // Read item - Team
            mockTeamRepo.Setup(r => r.Read<Team>(It.IsAny<int>())).Returns<int>((id) => _teams.First(e => e.Key == id).Value);
            
            // Read items - Team
            mockTeamRepo.Setup(r => r.Read<Team>()).Returns(_teams.Values.AsQueryable());
            
            // Create - Team
            mockTeamRepo.Setup(r => r.Create<Team>(It.IsAny<Team>())).Callback<Team>(team =>
            {
                int nextId = id++;
                team.ID = nextId;
                _teams.Add(nextId, team);

            });
            
            // Update - Team
            mockTeamRepo.Setup(r => r.Update<Team>(It.IsAny<Team>())).Callback<Team>(team =>
            {
                if (_teams.ContainsKey(team.ID))
                {
                    _teams[team.ID] = team;
                }
            });
            
            // Delete - Team
            mockTeamRepo.Setup(r => r.Delete<Team>(It.IsAny<Team>())).Callback<Team>(team =>
            {
                _teams.Remove(team.ID);
            });

        }

        /// <summary>
        /// Tests if a team has been added to the mock repo
        /// </summary>

        [TestMethod]
        public void TestStorageAddTeam()
        {
            testTeamStorageManager.CreateTeam(testTeam);
            Assert.AreEqual(1, _teams.Values.ToList().Count);
        }

        /// <summary>
        /// Tests get on a team from the mock repo
        /// </summary>

        [TestMethod]
        public void TestStorageGetTeam() {
            testTeamStorageManager.CreateTeam(testTeam);
            Assert.AreEqual(testTeam, testTeamStorageManager.GetTeam(1));
            Assert.AreEqual(1, testTeamStorageManager.GetTeam(1).ID);
        }

        /// <summary>
        /// Tests get on all teams in the mock repo
        /// </summary>

        public void TestStorageGetAllTeams()
        {
            testTeamStorageManager.CreateTeam(testTeam);
            var testTeam2 = new Team();
            testTeamStorageManager.CreateTeam(testTeam2);
            Assert.AreEqual(2, testTeamStorageManager.GetAllTeams().Count());
        }

        /// <summary>
        /// Tests if a team has been removed to the mock repo
        /// </summary>

        [TestMethod]
        public void TestStorageRemoveTeam()
        {
            testTeamStorageManager.CreateTeam(testTeam);
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
        public void TestStorageNoTeamToRemove()
        {
            Assert.AreEqual(0, _teams.Values.ToList().Count);
            Assert.IsFalse(testTeamStorageManager.RemoveTeam(1));
        }

        /// <summary>
        /// Tests if a team is updated
        /// </summary>

        [TestMethod]
        public void TestStorageUpdateTeam()
        {
            var testTeam = new Team() { ID = 1, Name = "Team" };
            testTeamStorageManager.CreateTeam(testTeam);
            testTeam.Name = "Team Awesome";
            testTeamStorageManager.UpdateTeam(testTeam);
            Assert.AreEqual("Team Awesome", testTeamStorageManager.GetTeam(1).Name);
        }
    }
}