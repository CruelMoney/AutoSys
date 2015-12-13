using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Models;

namespace LogicTests1.TeamCRUD
{
    [TestClass()]
    public class UserManagementTests
    {
        [TestMethod()]
        public void TestUserCreate()
        {
            User testUser = new User() { ID = 1, Name = "Test Name"};
            Assert.AreEqual("Test Name", testUser.Name);
        }
    }
}