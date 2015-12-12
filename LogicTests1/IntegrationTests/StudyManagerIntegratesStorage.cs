using StudyConfigurationServer.Logic.StorageManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicTests1.IntegrationTests.DBInitializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Storage.Repository;
using StudyConfigurationServer.Logic.StudyConfiguration;
using StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;

namespace LogicTests1.IntegrationTests
{

    [TestClass]
    public class StudyManagerIntegratesStorageAndTaskManager
    {
    
        StudyManager _manager;
        EntityFrameworkGenericRepository<StudyContext> _repo;

        [TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer(new MultipleTeamsDB());
            var context = new StudyContext();
            context.Database.Initialize(true);

            _repo = new EntityFrameworkGenericRepository<StudyContext>();
          

            _manager = new StudyManager(_repo);
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
                Rule = CriteriaDTO.CriteriaRule.AfterYear,
                DataMatch = new string[] {"2000"},
                DataType = DataFieldDTO.DataType.String,
                Description = "Check if the year is before 2000",
            };

            var criteria2 = new CriteriaDTO()
            {
                Name = "Is about...",
                DataType = DataFieldDTO.DataType.Boolean,
                Rule = CriteriaDTO.CriteriaRule.Equals,
                DataMatch = new string[] {"true"},
                Description = "Check if the item is about snails.",
            };

            var stage1 = new StageDTO()
            {
                Name = "stage1",
                Criteria = new CriteriaDTO[] {criteria1},
                DistributionRule = StageDTO.Distribution.HundredPercentOverlap,
                ReviewerIDs = new int[] {1, 2},
                ValidatorIDs = new int[] {3},
                VisibleFields = new StageDTO.FieldType[] {StageDTO.FieldType.Title, StageDTO.FieldType.Author,},

            };

            var stage2 = new StageDTO()
            {
                Name = "stage2",
                Criteria = new CriteriaDTO[] {criteria2},
                DistributionRule = StageDTO.Distribution.HundredPercentOverlap,
                ReviewerIDs = new int[] {3, 2},
                ValidatorIDs = new int[] {4},
                VisibleFields = new StageDTO.FieldType[] {StageDTO.FieldType.Title, StageDTO.FieldType.Author,},

            };

            var studyDTO = new StudyDTO()
            {
                Name = "testStudy",
                Team = teamDTO,
                Items = Properties.Resources.bibtex,
                Stages = new StageDTO[] {stage1, stage2}
            };

            return studyDTO;
        }


        [TestMethod]
        public void CreateStudyTest()
        {
            //Arrange 
            var studyDto = CreaStudyDto();

            //Action
            var studyID = _manager.CreateStudy(studyDto);

            var newStorageManager = new StudyStorageManager();

            var actualStudy = newStorageManager.GetAllStudies()
                .Where(s=>s.Id==studyID)
                .Include(s=>s.Stages.Select(t=>t.Tasks))
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
        public void GetStudyTask()
        {
            //Arrange 
            var studyDto = CreaStudyDto();
            var studyID = _manager.CreateStudy(studyDto);

            var newManager = new StudyManager();

            //Action
            var tasks = newManager.getTasks(1, 1, 20, TaskRequestDTO.Filter.Remaining, TaskRequestDTO.Type.Review).ToList();


        }

    }


}

