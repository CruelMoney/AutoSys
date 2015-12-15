#region Using

using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.TeamCRUD;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests
{
    [TestClass]
    public class UserManagerIntegrateStorage
    {
        private readonly UserDto _userDto = new UserDto {Id = 1, Name = "Bob"};
        private TeamStorageManager _teamStorageManager;
        private IGenericRepository _testRepo;
        private UserManager _userManager;

        [TestInitialize]
        public void InitializeTest()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<IntegrationTestContext>());
            var testContext = new IntegrationTestContext();
            testContext.Database.Initialize(true);

            _testRepo = new EntityFrameworkGenericRepository<IntegrationTestContext>(testContext);
            _teamStorageManager = new TeamStorageManager(_testRepo);
            _userManager = new UserManager(_teamStorageManager);
        }

        [TestMethod]
        public void TestUserManagerIntegrationCreateUser()
        {
            _userManager.CreateUser(_userDto);
            Assert.AreEqual("Bob", _userManager.GetUserDto(_userDto.Id).Name);
        }

        [TestMethod]
        public void TestUserManagerIntegrationRemoveUser()
        {
            _userManager.CreateUser(_userDto);

            Assert.IsTrue(_userManager.RemoveUser(_userDto.Id));
        }

        [TestMethod]
        public void TestUserManagerIntegrationUpdateUser()
        {
            _userManager.CreateUser(_userDto);
            _userDto.Name = "Bob2";
            Assert.IsTrue(_userManager.UpdateUser(_userDto.Id, _userDto));
            Assert.AreEqual("Bob2", _userManager.GetUserDto(_userDto.Id).Name);
        }

        [TestMethod]
        public void TestUserManagerIntegrationSearchUsers()
        {
            _userManager.CreateUser(_userDto);
            Assert.AreEqual(1, _userManager.SearchUserDtOs("Bob").Count());
        }

        [TestMethod]
        public void TestUserManagerIntegrationGetAllUsers()
        {
            var userDto2 = new UserDto {Id = 2, Name = "Bob2"};
            _userManager.CreateUser(_userDto);
            _userManager.CreateUser(userDto2);
            Assert.AreEqual(2, _userManager.GetAllUserDtOs().Count());
        }
    }
}