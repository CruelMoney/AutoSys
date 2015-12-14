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

namespace LogicTests1.IntegrationTests
{
    [TestClass]
    public class UserControllerIntegrationTest
    {
        private UserManager userManager;
        private TeamStorageManager userStorageManager;
        //private UserController userController;
        private User user = new User() { ID = 1, Name = "Bob" };
        private UserDTO userDTO = new UserDTO() { Id = 1, Name = "Bob"};
        private IGenericRepository testRepo;

        [TestInitialize]
        public void InitializeTest() {
            Database.SetInitializer(new DropCreateDatabaseAlways<IntegrationTestContext>());
            IntegrationTestContext testContext = new IntegrationTestContext();
            testContext.Database.Initialize(true);

            testRepo = new EntityFrameworkGenericRepository<IntegrationTestContext>(testContext);
            userStorageManager = new TeamStorageManager(testRepo);
            userManager = new UserManager(userStorageManager);
            //userController = new UserController();
            
        }

        [TestMethod]
        public void TestIntegrationAddUser()
        {

            userManager.CreateUser(userDTO);
            Assert.AreEqual("Bob", userManager.GetUserDTO(userDTO.Id).Name);
        }
    }
}
