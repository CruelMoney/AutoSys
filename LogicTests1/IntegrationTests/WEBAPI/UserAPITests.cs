using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Api;
using System.Data.Entity;
using LogicTests1.IntegrationTests.DBInitializers;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Logic.TeamCRUD;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Security.Cryptography.X509Certificates;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServer.Logic.StudyConfiguration;

namespace LogicTests1.IntegrationTests.WEBAPI
{
    [TestClass]
    public class UserAPITests
    {
        UserController _API;

        [TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer(new MultipleTeamsDB());

            var context = new StudyContext();
            context.Database.Initialize(true);

            var studyManager = new StudyManager();

            studyManager.CreateStudy(CreaStudyDto());

            _API = new UserController();
           

        }

        public StudyDTO CreaStudyDto()
        {
            var teamDTO = new TeamDTO()
            {
                Id = 1
            };

            var criteria1 = new CriteriaDTO()
            {
                Name = "Year",
                Rule = CriteriaDTO.CriteriaRule.BeforeDate,
                DataMatch = new string[] { "2000" },
                DataType = DataFieldDTO.DataType.String,
                Description = "Write the year of the study",
            };

            var criteria2 = new CriteriaDTO()
            {
                Name = "Is about...",
                DataType = DataFieldDTO.DataType.Boolean,
                Rule = CriteriaDTO.CriteriaRule.Equals,
                DataMatch = new string[] { "true" },
                Description = "Check if the item is about snails.",
            };

            var stage1 = new StageDTO()
            {
                Name = "stage1",
                Criteria = criteria1,
                DistributionRule = StageDTO.Distribution.HundredPercentOverlap,
                ReviewerIDs = new int[] { 1, 2 },
                ValidatorIDs = new int[] { 3 },
                VisibleFields = new StageDTO.FieldType[] { StageDTO.FieldType.Title, StageDTO.FieldType.Author, StageDTO.FieldType.Year },

            };

            var stage2 = new StageDTO()
            {
                Name = "stage2",
                Criteria = criteria2,
                DistributionRule = StageDTO.Distribution.HundredPercentOverlap,
                ReviewerIDs = new int[] { 3, 2 },
                ValidatorIDs = new int[] { 4 },
                VisibleFields = new StageDTO.FieldType[] { StageDTO.FieldType.Title, StageDTO.FieldType.Author, StageDTO.FieldType.Year },

            };

            var studyDTO = new StudyDTO()
            {
                Name = "testStudy",
                Team = teamDTO,
                Items = Properties.Resources.bibtex,
                Stages = new StageDTO[] { stage1, stage2 }
            };

            return studyDTO;
        }

        [TestMethod]
        public void TestGetUsers()
        {
            //action
            var result = _API.Get();

            //assert
            OkNegotiatedContentResult<IEnumerable<UserDTO>> negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<UserDTO>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(8, negotiatedResult.Content.Count());
        }

        [TestMethod]
        public void TestGetUserByName()
        {
            var result = _API.Get("ramos");

            OkNegotiatedContentResult<IEnumerable<UserDTO>> negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<UserDTO>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(1, negotiatedResult.Content.Count());
        }

                
        

        [TestMethod]
        public void TestGetUserByExistingId()
        {
            var result = _API.Get(1);

            OkNegotiatedContentResult<UserDTO> negotiatedResult = result as OkNegotiatedContentResult<UserDTO>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual("chris", negotiatedResult.Content.Name);
        }

        

        [TestMethod]
        public void TestTryGetinvalidUser()
        {
            var result = _API.Get(20);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetStudyIDsFromUserId()
        {
            var result = _API.GetStudyIDs(1);

            OkNegotiatedContentResult<IEnumerable<int>> negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<int>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(1, negotiatedResult.Content.Count());
        }

        [TestMethod]
        public void TestPostUser()
        {
            var newUser = new UserDTO() {Name = "TestUser" };

            var result = _API.Post(newUser);

            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<UserDTO>));
            CreatedAtRouteNegotiatedContentResult<UserDTO> negotiatedResult= result as CreatedAtRouteNegotiatedContentResult<UserDTO>;
            Assert.IsNotNull((negotiatedResult));
            Assert.AreEqual("TestUser", negotiatedResult.Content.Name);
            Assert.AreEqual(9, negotiatedResult.Content.Id);
        }

        [TestMethod]
        public void TestPutValidUser()
        {
            var newUser = new UserDTO() {Name = "NewName"};

            var result = _API.Put(1, newUser);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
            

        }

        [TestMethod]
        public void TestPutUSerInvalidUser()
        {
            var newUser = new UserDTO() {Name = "newName"};
            var result = _API.Put(50, newUser);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
           
        }

        [TestMethod]
        public void TestDeleteUserInAStudy()
        {
            var result = _API.Delete(1);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void TestDeleteValidUser()
        {
            var userToDelete = new UserDTO() {Name = "BadName"};

            _API.Post(userToDelete);

            var result = _API.Delete(9);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
        }

        [TestMethod]
        public void TestDeleteInvalidUser()
        {
            var result = _API.Delete(50);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }



    }

}
