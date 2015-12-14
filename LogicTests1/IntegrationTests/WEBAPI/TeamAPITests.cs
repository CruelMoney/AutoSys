using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Results;
using LogicTests1.IntegrationTests.DBInitializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Api;
using StudyConfigurationServer.Logic.StudyConfiguration;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;


namespace LogicTests1.IntegrationTests.WEBAPI
{
    [TestClass]
    public class TeamAPITests
    {
        TeamController _API = new TeamController();

        

        [TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer(new MultipleTeamsDB());

            var context = new StudyContext();
            context.Database.Initialize(true);

            var studyManager = new StudyManager();

            studyManager.CreateStudy(CreaStudyDto());
        }

        public StudyDTO CreaStudyDto()
        {
            var teamDTO = new TeamDTO()
            {
                Id = 1
            };
            var studyDTO = new StudyDTO()
            {
                Name = "testStudy",
                Team = teamDTO,
                Items = Properties.Resources.bibtex,
                Stages = new StageDTO[] {}
            };

            return studyDTO;
        }

        [TestMethod]
        public void TestGetAllTeams()
        {
            var result = _API.Get();

            OkNegotiatedContentResult<IEnumerable<TeamDTO>> NegotiatedResult = result as OkNegotiatedContentResult<IEnumerable<TeamDTO>>;
            Assert.IsNotNull(NegotiatedResult);
            Assert.AreEqual(3, NegotiatedResult.Content.Count());
        }

        [TestMethod]
        public void TestGetTeamByID()
        {
            var result = _API.Get(1);

            OkNegotiatedContentResult<TeamDTO> negotiatedResult = result as OkNegotiatedContentResult<TeamDTO>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual("team1", negotiatedResult.Content.Name);
        }

        [TestMethod]
        public void TestGetInvalidTeamById()
        {
            var result = _API.Get(50);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void TestPostValidTeam()
        {
            var newTeam = new TeamDTO() {Name = "potatoTeam", UserIDs = new int[] {1,2}};

            var result = _API.Post(newTeam);

            Assert.IsInstanceOfType(result, typeof(CreatedAtRouteNegotiatedContentResult<TeamDTO>));
            CreatedAtRouteNegotiatedContentResult<TeamDTO> negotiatedResult = result as CreatedAtRouteNegotiatedContentResult<TeamDTO>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual("potatoTeam", negotiatedResult.Content.Name);
            Assert.AreEqual(4, negotiatedResult.Content.Id);
        }

        [TestMethod]
        public void TestPutValidTeam()
        {
            var newTeam = new TeamDTO() {Name = "newName", UserIDs = new int[] {1,2,3,4}};

            var result = _API.Put(1, newTeam);

            Assert.IsInstanceOfType(result, typeof(StatusCodeResult));
        }

        [TestMethod]
        public void TestPutInvalidTeam()
        {
            var newTeam = new TeamDTO() {Name = "newName", UserIDs = new int[] {1,3}};
            var result = _API.Put(1, newTeam);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void TestPutTeamInvalidTeamId()
        {
            var newTeam = new TeamDTO() {Name = "newName"};
            var result = _API.Put(50, newTeam);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void TestDeleteTeamInAStudy()
        {
            var result = _API.Delete(1);

            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void TestDeleteValidTeam()
        {
            var teamToDelete = new TeamDTO() {Name = "potatoTeam", UserIDs = new int[] {1,2}};
            _API.Post(teamToDelete);

            var result = _API.Delete(4);
        }

        [TestMethod]
        public void TestDeleteInvalidTeam()
        {
            var result = _API.Delete(50);

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        




    }
}
