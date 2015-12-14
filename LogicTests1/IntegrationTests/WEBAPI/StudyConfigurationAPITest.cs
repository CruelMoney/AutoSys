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
    public class StudyConfigurationAPITest
    {
        StudyConfigurationController _apiTest;

        [TestInitialize]
        public void Initialize()
        {
            Database.SetInitializer(new MultipleTeamsDB());

            var context = new StudyContext();
            context.Database.Initialize(true);

            _apiTest = new StudyConfigurationController();

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
        public void CreateStudyTest()
        {
            throw new NotImplementedException();
            

        }

    }
}
