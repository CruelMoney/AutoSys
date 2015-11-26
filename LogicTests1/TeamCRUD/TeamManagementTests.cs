﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logic.TeamCRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.TeamCRUD.Tests
{
    [TestClass()]
    public class TeamManagementTests
    {
        [TestMethod()]
        public void TeamManagementTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void TeamManagementTest1()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void CreateTeamTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void AddUserToTeamTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void RemoveUserFromTeamTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void UpdateTeamTest()
        {
            throw new NotImplementedException();
        }

        [TestMethod()]
        public void RetrieveTeamTest()
        {
            var TeamManager = new TeamManagement();
            int[] UserIDs = new int[] { 1, 2, 3, 4 };
            TeamManager.CreateTeam("SuperTeam", UserIDs, "some metadata");
            Assert.AreSame(TeamManager.GetTeam(0).UserIDs, UserIDs);
            Assert.AreSame(TeamManager.GetTeam(0).Name, "SuperTeam");
            Assert.AreSame(TeamManager.GetTeam(0).Metadata, "some metadata");
        }

        [TestMethod()]
        public void DeleteTeamTest()
        {
            throw new NotImplementedException();
        }
    }
}