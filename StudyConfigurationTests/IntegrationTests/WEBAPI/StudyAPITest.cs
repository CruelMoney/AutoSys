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
using StudyConfigurationServerTests.Properties;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests.WEBAPI
{
    [TestClass]
    public class StudyApiTest
    {
        private StudyController _api;

        [TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer(new StudyDb());

            var context = new StudyContext();
            context.Database.Initialize(true);

            _api = new StudyController();
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
        public void GetOverviewTest()
        {
            //Action
            var result = _api.GetOverview(1);

            //Assert
            var negotiatedResult = result as OkNegotiatedContentResult<StudyOverviewDto>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(2, negotiatedResult.Content.Stages.Length);
            Assert.AreEqual("Test Study", negotiatedResult.Content.Name);
            Assert.AreEqual(4, negotiatedResult.Content.UserIds.Length);
        }

        [TestMethod]
        public void GetOverviewInvalidStudyTest()
        {
            //Action
            var result = _api.GetOverview(10);

            //Assert
            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }


        [TestMethod]
        public void GetTasksTest()
        {
            //Action
            var result = _api.GetTasks(1, 1, 1);

            //Assert
            var negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<TaskRequestDto>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(1, negotiatedResult.Content.Count());
        }

        [TestMethod]
        public void GetTasksInvalidStudyTest()
        {
            //Action
            var result = _api.GetTasks(10, 1);

            //Assert
            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }

        [TestMethod]
        public void GetTasksInvalidUserTest()
        {
            //Action
            var result = _api.GetTasks(1, 100);

            //Assert
            Assert.IsInstanceOfType(result, typeof (BadRequestResult));
        }


        [TestMethod]
        public void GetTaskIDsTest()
        {
            //Action
            var result = _api.GetTaskIDs(1, 1);

            //Assert
            var negotiatedResult = result as OkNegotiatedContentResult<IEnumerable<int>>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(2, negotiatedResult.Content.Count());
        }

        [TestMethod]
        public void GetTaskIDsInvalidStudyTest()
        {
            //Action
            var result = _api.GetTaskIDs(10, 1);

            //Assert
            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }

        [TestMethod]
        public void GetTaskIDsInvalidUserTest()
        {
            //Action
            var result = _api.GetTaskIDs(1, 100);

            //Assert
            Assert.IsInstanceOfType(result, typeof (BadRequestResult));
        }

        [TestMethod]
        public void GetTaskTest()
        {
            //Action
            var result = _api.GetTask(1, 1);

            //Assert
            var negotiatedResult = result as OkNegotiatedContentResult<TaskRequestDto>;
            Assert.IsNotNull(negotiatedResult);
            Assert.AreEqual(1, negotiatedResult.Content.Id);
            Assert.IsNotNull(negotiatedResult.Content.IsDeliverable);
            Assert.IsNotNull(negotiatedResult.Content.RequestedFields);
            Assert.IsNotNull(negotiatedResult.Content.VisibleFields);
            Assert.IsNotNull(negotiatedResult.Content.IsDeliverable);
        }

        [TestMethod]
        public void GetTaskInvalidTask()
        {
            //Action
            var result = _api.GetTask(1, 100);

            //Assert
            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }

        [TestMethod]
        public void PostTask()
        {
            var expectedData = "updatedData";

            //Arrange
            var taskSubmission = new TaskSubmissionDto
            {
                UserId = 1,
                SubmittedFields = new[]
                {
                    new DataFieldDto {Data = new[] {expectedData}, Name = "Year"}
                }
            };

            //Action
            var result = _api.Post(1, 1, taskSubmission);
            var taskResult = _api.GetTasks(1, 1, 1, TaskRequestDto.Filter.Editable);

            //Assert
            Assert.IsInstanceOfType(result, typeof (StatusCodeResult));
            var negotiatedResult = taskResult as OkNegotiatedContentResult<IEnumerable<TaskRequestDto>>;
            Assert.IsNotNull(negotiatedResult);
            var resultTask = negotiatedResult.Content.ToList();
            Assert.AreEqual(1, resultTask.First().Id);
            Assert.AreEqual(expectedData, resultTask.First().RequestedFields.First().Data[0]);
        }

        [TestMethod]
        public void PostTaskInvalidDataName()
        {
            var expectedData = "updatedData";

            //Arrange
            var taskSubmission = new TaskSubmissionDto
            {
                UserId = 1,
                SubmittedFields = new[]
                {
                    new DataFieldDto {Data = new[] {expectedData}, Name = "InvalidName"}
                }
            };

            //Action
            var result = _api.Post(1, 1, taskSubmission);

            //Assert
            Assert.IsInstanceOfType(result, typeof (BadRequestResult));
        }

        [TestMethod]
        public void PostTaskInvalidTask()
        {
            var expectedData = "updatedData";

            //Arrange
            var taskSubmission = new TaskSubmissionDto
            {
                UserId = 1,
                SubmittedFields = new[]
                {
                    new DataFieldDto {Data = new[] {expectedData}, Name = "Year"}
                }
            };

            //Action
            var result = _api.Post(1, 100, taskSubmission);

            //Assert
            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }

        [TestMethod]
        public void PostTaskInvalidUser()
        {
            var expectedData = "updatedData";

            //Arrange
            var taskSubmission = new TaskSubmissionDto
            {
                UserId = 100,
                SubmittedFields = new[]
                {
                    new DataFieldDto {Data = new[] {expectedData}, Name = "Year"}
                }
            };

            //Action
            var result = _api.Post(1, 1, taskSubmission);

            //Assert
            Assert.IsInstanceOfType(result, typeof (BadRequestResult));
        }

        [TestMethod]
        public void PostTaskInvalidStudy()
        {
            var expectedData = "updatedData";

            //Arrange
            var taskSubmission = new TaskSubmissionDto
            {
                UserId = 100,
                SubmittedFields = new[]
                {
                    new DataFieldDto {Data = new[] {expectedData}, Name = "Year"}
                }
            };

            //Action
            var result = _api.Post(100, 1, taskSubmission);

            //Assert
            Assert.IsInstanceOfType(result, typeof (NotFoundResult));
        }
    }
}