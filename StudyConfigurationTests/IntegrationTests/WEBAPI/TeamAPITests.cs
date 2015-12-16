#region Using

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudyConfigurationServer.Api;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServerTests.IntegrationTests.DBInitializers;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests.WEBAPI
{
    [TestClass]
    public class TeamApiTests
    {
        private readonly TeamController _api = new TeamController();

        [TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer(new MultipleTeamsDb());

            var context = new StudyContext();
            context.Database.Initialize(true);
        }


        [TestMethod]
        public void TestGetAllTeams()
        {
            var result = _api.Get();

            var negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<TeamDto>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(3, negotiatedResult.Content.Count());
        }

        [TestMethod]
        public void TestGetTeamById()
        {
            var result = _api.Get(1);

            var negotiatedResult = result as OkNegotiatedContentResult<TeamDto>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual("team1", negotiatedResult.Content.Name);
        }

        [TestMethod]
        public void TestGetInvalidTeamById()
        {
            var result = _api.Get(50);

            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }

        [TestMethod]
        public void TestPostValidTeam()
        {
            var newTeam = new TeamDto {Name = "potatoTeam", UserIDs = new[] {1, 2}};

            var result = _api.Post(newTeam);

            Assert.IsInstanceOfType(result, typeof (CreatedAtRouteNegotiatedContentResult<TeamDto>));
            var negotiatedResult = result as CreatedAtRouteNegotiatedContentResult<TeamDto>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual("potatoTeam", negotiatedResult.Content.Name);
            Assert.AreEqual(4, negotiatedResult.Content.Id);
        }

        [TestMethod]
        public void TestPutValidTeam()
        {
            var newTeam = new TeamDto {Name = "newName", UserIDs = new[] {1, 2, 3, 4}};

            var result = _api.Put(1, newTeam);

            Assert.IsInstanceOfType(result, typeof (StatusCodeResult));
        }

        [TestMethod]
        public void TestPutInvalidTeam()
        {
            var newTeam = new TeamDto {Name = "newName", UserIDs = new[] {1, 3}};
            var result = _api.Put(1, newTeam);

            Assert.IsInstanceOfType(result, typeof (BadRequestResult));
        }

        [TestMethod]
        public void TestPutTeamInvalidTeamId()
        {
            var newTeam = new TeamDto {Name = "newName"};
            var result = _api.Put(50, newTeam);

            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }

        [TestMethod]
        public void TestDeleteTeamInAStudy()
        {
            Database.SetInitializer(new StudyDb());

            var context = new StudyContext();
            context.Database.Initialize(true);

            var result = _api.Delete(1);

            Assert.IsInstanceOfType(result, typeof (BadRequestResult));
        }

        [TestMethod]
        public void TestDeleteValidTeam()
        {
            var teamToDelete = new TeamDto {Name = "potatoTeam", UserIDs = new[] {1, 2}};
            _api.Post(teamToDelete);

            var result = _api.Delete(4);
        }

        [TestMethod]
        public void TestDeleteInvalidTeam()
        {
            var result = _api.Delete(50);

            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }
    }
}