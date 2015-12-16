#region Using

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Api;
using StudyConfigurationServer.Logic.StudyManagement;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServerTests.IntegrationTests.DBInitializers;
using StudyConfigurationServerTests.Properties;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests.WEBAPI
{
    [TestClass]
    public class UserApiTests
    {
        private UserController _api;

        [TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer(new MultipleTeamsDb());

            var context = new StudyContext();
            context.Database.Initialize(true);

            var studyManager = new StudyManager();

            studyManager.CreateStudy(CreaStudyDto());

            _api = new UserController();
        }

        public StudyDto CreaStudyDto()
        {
            var teamDto = new TeamDto
            {
                Id = 1
            };

            var criteria1 = new CriteriaDto
            {
                Name = "Year",
                Rule = CriteriaDto.CriteriaRule.LargerThan,
                DataMatch = new[] {"2000"},
                DataType = DataFieldDto.DataType.String,
                Description = "Write the year of the study"
            };

            var criteria2 = new CriteriaDto
            {
                Name = "Is about...",
                DataType = DataFieldDto.DataType.Boolean,
                Rule = CriteriaDto.CriteriaRule.Equals,
                DataMatch = new[] {"true"},
                Description = "Check if the item is about snails."
            };

            var stage1 = new StageDto
            {
                Name = "stage1",
                Criteria = criteria1,
                DistributionRule = StageDto.Distribution.HundredPercentOverlap,
                ReviewerIDs = new[] {1, 2},
                ValidatorIDs = new[] {3},
                VisibleFields = new[] {StageDto.FieldType.Title, StageDto.FieldType.Author, StageDto.FieldType.Year}
            };

            var stage2 = new StageDto
            {
                Name = "stage2",
                Criteria = criteria2,
                DistributionRule = StageDto.Distribution.HundredPercentOverlap,
                ReviewerIDs = new[] {3, 2},
                ValidatorIDs = new[] {4},
                VisibleFields = new[] {StageDto.FieldType.Title, StageDto.FieldType.Author, StageDto.FieldType.Year}
            };

            var studyDto = new StudyDto
            {
                Name = "testStudy",
                Team = teamDto,
                Items = Resources.bibtex,
                Stages = new[] {stage1, stage2}
            };

            return studyDto;
        }

        [TestMethod]
        public void TestGetUsers()
        {
            //action
            var result = _api.Get();

            //assert
            var negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<UserDto>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(8, negotiatedResult.Content.Count());
        }

        [TestMethod]
        public void TestGetUserByName()
        {
            var result = _api.Get("ramos");

            var negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<UserDto>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(1, negotiatedResult.Content.Count());
        }


        [TestMethod]
        public void TestGetUserByExistingId()
        {
            var result = _api.Get(1);

            var negotiatedResult = result as OkNegotiatedContentResult<UserDto>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual("chris", negotiatedResult.Content.Name);
        }


        [TestMethod]
        public void TestTryGetinvalidUser()
        {
            var result = _api.Get(20);

            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }

        [TestMethod]
        public void GetStudyIDsFromUserId()
        {
            var result = _api.GetStudyIDs(1);

            var negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<int>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(1, negotiatedResult.Content.Count());
        }

        [TestMethod]
        public void TestPostUser()
        {
            var newUser = new UserDto {Name = "TestUser"};

            var result = _api.Post(newUser);

            Assert.IsInstanceOfType(result, typeof (CreatedAtRouteNegotiatedContentResult<UserDto>));
            var negotiatedResult = result as CreatedAtRouteNegotiatedContentResult<UserDto>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual("TestUser", negotiatedResult.Content.Name);
            Assert.AreEqual(9, negotiatedResult.Content.Id);
        }

        [TestMethod]
        public void TestPutValidUser()
        {
            var newUser = new UserDto {Name = "NewName"};

            var result = _api.Put(1, newUser);

            Assert.IsInstanceOfType(result, typeof (StatusCodeResult));
        }

        [TestMethod]
        public void TestPutUSerInvalidUser()
        {
            var newUser = new UserDto {Name = "newName"};
            var result = _api.Put(50, newUser);

            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }

        [TestMethod]
        public void TestDeleteUserInAStudy()
        {
            var result = _api.Delete(1);

            Assert.IsInstanceOfType(result, typeof (BadRequestResult));
        }

        [TestMethod]
        public void TestDeleteValidUser()
        {
            var userToDelete = new UserDto {Name = "BadName"};

            _api.Post(userToDelete);

            var result = _api.Delete(9);

            Assert.IsInstanceOfType(result, typeof (StatusCodeResult));
        }

        [TestMethod]
        public void TestDeleteInvalidUser()
        {
            var result = _api.Delete(50);

            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }
    }
}