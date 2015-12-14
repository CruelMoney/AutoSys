using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
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
    public class StudyAPITest
    {
        StudyController _API;

        [TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer(new MultipleTeamsDB());

            var context = new StudyContext();
            context.Database.Initialize(true);


            var studyManager = new StudyManager();

            studyManager.CreateStudy(CreaStudyDto());

            _API = new StudyController();

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
                Rule = CriteriaDTO.CriteriaRule.BeforeYear,
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
        public void GetOverviewTest()
        {
            //Action
            var result = _API.GetOverview(1);

            //Assert
            OkNegotiatedContentResult<StudyOverviewDTO> negotiatedResult = result as OkNegotiatedContentResult<StudyOverviewDTO>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(2, negotiatedResult.Content.Phases.Length);
            Assert.AreEqual("testStudy", negotiatedResult.Content.Name);
            Assert.AreEqual(4, negotiatedResult.Content.UserIds.Length);
        }

        [TestMethod]
        public void GetOverviewInvalidStudyTest()
        {
            //Action
            var result = _API.GetOverview(10);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }


        [TestMethod]
        public void GetTasksTest()
        {
            //Action
            var result = _API.GetTasks(1, 1);

            //Assert
            OkNegotiatedContentResult<IEnumerable<TaskRequestDTO>> negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<TaskRequestDTO>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(23, negotiatedResult.Content.Count());
        }

        [TestMethod]
        public void GetTasksInvalidStudyTest()
        {
            //Action
            var result = _API.GetTasks(10, 1);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTasksInvalidUserTest()
        {
            //Action
            var result = _API.GetTasks(1, 100);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }


        [TestMethod]
        public void GetTaskIDsTest()
        {
            //Action
            var result = _API.GetTaskIDs(1, 1);

            //Assert
            OkNegotiatedContentResult<IEnumerable<int>> negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<int>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(23, negotiatedResult.Content.Count());
        }

        [TestMethod]
        public void GetTaskIDsInvalidStudyTest()
        {
            //Action
            var result = _API.GetTaskIDs(10, 1);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTaskIDsInvalidUserTest()
        {
            //Action
            var result = _API.GetTaskIDs(1, 100);

            //Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));

        }

        [TestMethod]
        public void GetTaskTest()
        {
            //Action
            var result = _API.GetTask(1, 1);

            //Assert
            OkNegotiatedContentResult<TaskRequestDTO> negotiatedResult = result as OkNegotiatedContentResult<TaskRequestDTO>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(1, negotiatedResult.Content.Id);
            Assert.IsNotNull(negotiatedResult.Content.IsDeliverable);
            Assert.IsNotNull(negotiatedResult.Content.RequestedFieldsDto);
            Assert.IsNotNull(negotiatedResult.Content.VisibleFieldsDto);
            Assert.IsNotNull(negotiatedResult.Content.IsDeliverable);
        }

        [TestMethod]
        public void GetTaskInvalidStudyTest()
        {
            //Action
            var result = _API.GetTaskIDs(10, 1);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTaskInvalidTaskID()
        {
            //Action
            var result = _API.GetTaskIDs(1, 100);

            //Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));

        }


    }
}
