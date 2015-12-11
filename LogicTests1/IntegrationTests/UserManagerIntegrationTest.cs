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
    public class UserManagerIntegrationTest
    {
        private UserManager userManager;
        private TeamStorageManager teamStorageManager;

        private UserDTO userDTO = new UserDTO() { Id = 1, Name = "Bob" };
        private IGenericRepository testRepo;

        [TestInitialize]
        public void InitializeTest()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<IntegrationTestContext>());
            IntegrationTestContext testContext = new IntegrationTestContext();
            testContext.Database.Initialize(true);

            testRepo = new EntityFrameworkGenericRepository<IntegrationTestContext>(testContext);
            teamStorageManager = new TeamStorageManager(testRepo);
            userManager = new UserManager(teamStorageManager);

        }

        [TestMethod]
        public void TestUserManagerIntegrationCreateUser()
        {

            userManager.CreateUser(userDTO);
            Assert.AreEqual("Bob", userManager.GetUserDTO(userDTO.Id).Name);
        }

        [TestMethod]
        public void TestUserManagerIntegrationRemoveUser()
        {

            userManager.CreateUser(userDTO);

            Assert.IsTrue(userManager.RemoveUser(userDTO.Id));
        }

        [TestMethod]
        public void TestUserManagerIntegrationUpdateUser()
        {
            userManager.CreateUser(userDTO);
            userDTO.Name = "Bob2";
            Assert.IsTrue(userManager.UpdateUser(userDTO.Id, userDTO));
            Assert.AreEqual("Bob2", userManager.GetUserDTO(userDTO.Id).Name);
        }

        [TestMethod]
        public void TestUserManagerIntegrationSearchUsers()
        {
            userManager.CreateUser(userDTO);
            Assert.AreEqual(1, userManager.SearchUserDTOs("Bob").Count());
        }

        [TestMethod]
        public void TestUserManagerIntegrationGetAllUsers()
        {
            UserDTO userDTO2 = new UserDTO() { Id = 2, Name = "Bob2" };
            userManager.CreateUser(userDTO);
            userManager.CreateUser(userDTO2);
            Assert.AreEqual(2, userManager.GetAllUserDTOs().Count());
        }
    }
}