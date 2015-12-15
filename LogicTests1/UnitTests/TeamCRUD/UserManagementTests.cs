#region Using

using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.UnitTests.TeamCRUD
{
    [TestClass]
    public class UserManagementTests
    {
        [TestMethod]
        public void TestUserCreate()
        {
            var testUser = new User {ID = 1, Name = "Test Name"};
            Assert.AreEqual("Test Name", testUser.Name);
        }
    }
}