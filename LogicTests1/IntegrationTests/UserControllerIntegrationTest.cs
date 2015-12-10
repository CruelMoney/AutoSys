﻿using System;
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
        public void TestIntegrationAddUser()
        {

            userManager.CreateUser(userDTO);
            Assert.AreEqual("Bob", userManager.GetUser(userDTO.Id).Name);
        }

        [TestMethod]
        public void TestIntegrationRemoveUser()
        {

            userManager.CreateUser(userDTO);
            
            Assert.IsTrue(userManager.RemoveUser(userDTO.Id));
        }

        [TestMethod]
        public void TestIntegrationUpdateUser()
        {

            userManager.CreateUser(userDTO);
            userDTO.Name = "Bob Sveskebob";
            Assert.IsTrue(userManager.UpdateUser(userDTO.Id, userDTO));
            Assert.AreEqual("Bob Sveskebob", userManager.GetUser(userDTO.Id).Name);
        }
    }
}
