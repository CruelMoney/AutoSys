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
        private UserStorageManager userStorageManager;
        
        private UserDTO userDTO = new UserDTO() { Id = 1, Name = "Bob"};
        private IGenericRepository testRepo;

        [TestInitialize]
        public void InitializeTest() {
            Database.SetInitializer(new DropCreateDatabaseAlways<IntegrationTestContext>());
            IntegrationTestContext testContext = new IntegrationTestContext();
            testContext.Database.Initialize(true);

            testRepo = new EntityFrameworkGenericRepository<IntegrationTestContext>(testContext);
            userStorageManager = new UserStorageManager(testRepo);
            userManager = new UserManager(userStorageManager);
            
        }

        [TestMethod]
        public void TestIntegrationUserManagerCreateUser()
        {

            userManager.CreateUser(userDTO);
            Assert.AreEqual("Bob", userManager.GetUser(userDTO.Id).Name);
        }

        [TestMethod]
        public void TestIntegrationUserManagerRemoveUser()
        {

            userManager.CreateUser(userDTO);
            
            Assert.IsTrue(userManager.RemoveUser(userDTO.Id));
        }

        [TestMethod]
        public void TestIntegrationUserManagerUpdateUser()
        {

            userManager.CreateUser(userDTO);
            userDTO.Name = "Bob Sveskebob";
            Assert.IsTrue(userManager.UpdateUser(userDTO.Id, userDTO));
            Assert.AreEqual("Bob Sveskebob", userManager.GetUser(userDTO.Id).Name);
        }

        [TestMethod]
        public void TestIntegrationUserManagerSearchUsers()
        {
            userManager.CreateUser(userDTO);
            Assert.AreEqual(1, userManager.SearchUsers("Bob").Count());
        }

        [TestMethod]
        public void TestIntegrationUserManagerGetAllUsers()
        {
            UserDTO userDTO2 = new UserDTO() { Id = 2, Name = "Bob2" };

            userManager.CreateUser(userDTO);
            userManager.CreateUser(userDTO2);
        
            Assert.AreEqual(2, userManager.GetAllUsers().Count());
        }

    }
}
