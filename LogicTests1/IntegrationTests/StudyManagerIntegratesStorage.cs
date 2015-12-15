#region Using

using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.StudyConfiguration;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServerTests.IntegrationTests.DBInitializers;
using StudyConfigurationServerTests.Properties;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests
{
    [TestClass]
    public class StudyManagerIntegratesStorageAndTaskManager
    {
        private StudyManager _manager;
        private EntityFrameworkGenericRepository<StudyContext> _repo;

        [TestInitialize]
        public void Initialize()
        {
            _manager = new StudyManager();
        }

        private void InitializeStudyDb()
        {
            Database.SetInitializer(new StudyDb());
            var context = new StudyContext();
            context.Database.Initialize(true);
        }

        private void InitializeTeamDb()
        {
            Database.SetInitializer(new MultipleTeamsDb());
            var context = new StudyContext();
            context.Database.Initialize(true);
        }

        public StudyDto CreaStudyDto()
        {
            var teamDto = new TeamDto
            {
                Id = 1
            };

            var criteria1 = new CriteriaDto
            {
                Name = "Name1",
                Rule = CriteriaDto.CriteriaRule.SmallerThan,
                DataMatch = new[] {"2000"},
                DataType = DataFieldDto.DataType.String,
                Description = "Check if the year is before 2000"
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
                VisibleFields = new[] {StageDto.FieldType.Title, StageDto.FieldType.Author}
            };

            var stage2 = new StageDto
            {
                Name = "stage2",
                Criteria = criteria2,
                DistributionRule = StageDto.Distribution.HundredPercentOverlap,
                ReviewerIDs = new[] {3, 2},
                ValidatorIDs = new[] {4},
                VisibleFields = new[] {StageDto.FieldType.Title, StageDto.FieldType.Author}
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
        public void CreateStudyTest()
        {
            //Arrange 
            InitializeTeamDb();
            var studyDto = CreaStudyDto();

            //Action
            var studyId = _manager.CreateStudy(studyDto);

            var newStorageManager = new StudyStorageManager();

            var actualStudy = newStorageManager.GetAll()
                .Where(s => s.ID == studyId)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

            var actualCurrentStage = actualStudy.CurrentStage();

            //Assert
            Assert.AreEqual("testStudy", actualStudy.Name);
            Assert.AreEqual("stage1", actualCurrentStage.Name);
            Assert.AreEqual(false, actualStudy.IsFinished);
            Assert.AreEqual(23, actualStudy.Items.Count);
            Assert.AreEqual(2, actualStudy.Stages.Count);
            Assert.AreEqual("team1", actualStudy.Team.Name);
            Assert.AreEqual(23, actualCurrentStage.Tasks.Count);
        }


        [TestMethod]
        public void DeliverTask()
        {
            //Arrange
            InitializeStudyDb();

            var taskDto = new TaskSubmissionDto
            {
                UserId = 1,
                SubmittedFieldsDto = new[]
                {
                    new DataFieldDto {Data = new[] {"updatedData"}, Name = "Year"}
                }
            };
            //Action
            _manager.DeliverTask(1, 1, taskDto);
            var result = _manager.GetTasks(1, 1, 1, TaskRequestDto.Filter.Editable, TaskRequestDto.Type.Review).ToList();
            var actualTask = result.First();

            //Assert
            Assert.AreEqual(1, actualTask.Id);
            Assert.AreEqual(true, actualTask.IsDeliverable);
            Assert.AreEqual("updatedData", actualTask.RequestedFieldsDto[0].Data[0]);
        }

        [TestMethod]
        public void RemoveStudyTest()
        {
            //Arrange
            InitializeStudyDb();

            //Action
            _manager.RemoveStudy(1);
        }

        [TestMethod]
        public void UpdateStudyTest()
        {
            //Arrange
            InitializeStudyDb();
            var study = _manager.GetStudy(1);

            var criteria1 = new CriteriaDto
            {
                Name = "updatedName",
                Rule = CriteriaDto.CriteriaRule.SmallerThan,
                DataMatch = new[] {"2000"},
                DataType = DataFieldDto.DataType.String,
                Description = "Check if the year is before 2000"
            };

            var stage1 = new StageDto
            {
                Name = "updatedStage",
                Criteria = criteria1,
                DistributionRule = StageDto.Distribution.HundredPercentOverlap,
                ReviewerIDs = new[] {1, 2},
                ValidatorIDs = new[] {3},
                VisibleFields = new[] {StageDto.FieldType.Title, StageDto.FieldType.Author}
            };

            //Action
            study.Name = "updatedName";
            study.Stages[study.Stages.Length - 1] = stage1;

            _manager.UpdateStudy(1, study);

            var result = _manager.GetStudy(1);

            //Assert
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("updatedName", result.Name);
            Assert.AreEqual(3, result.Stages.Length);
            Assert.AreEqual("updatedStage", result.Stages[2].Name);
        }
    }
}