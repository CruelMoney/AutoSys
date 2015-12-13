using System;
using System.Collections.Generic;
using System.Data.Entity;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServer.Logic.StudyConfiguration;

namespace LogicTests1.IntegrationTests.DBInitializers
{
    public class StudyDBInitializer : DropCreateDatabaseAlways<StudyContext>
    {
        protected override void Seed(StudyContext context)
        {

            //Here it is possible to initialize the db with a custom context

            var testUser1 = new User() { Id = 1, Name = "chris", };
            var testUser2 = new User() { Id = 2, Name = "ramos" };
            var testUser3 = new User() { Id = 3, Name = "kathrin" };
            var testUser4 = new User() { Id = 4, Name = "emil" };
            var testUser5 = new User() { Id = 1, Name = "user1" };
            var testUser6 = new User() { Id = 2, Name = "user2" };
            var testUser7 = new User() { Id = 3, Name = "user3" };
            var testUser8 = new User() { Id = 4, Name = "user4" };

            context.Users.AddRange(new List<User>() { testUser1, testUser2, testUser3, testUser4 });

            var testTeam1 = new Team() { Name = "team1", Users = new List<User>() { testUser1, testUser2, testUser3, testUser4 } };
            var testTeam2 = new Team() { Name = "team2", Users = new List<User>() { testUser5, testUser6, testUser7, testUser8 } };
            var testTeam3 = new Team() { Name = "team3", Users = new List<User>() { testUser1, testUser6, testUser3, testUser8 } };

            context.Teams.AddRange(new List<Team>() { testTeam1, testTeam2, testTeam3 });

            base.Seed(context);

            var studyManager = new StudyManager();

            studyManager.CreateStudy(CreaStudyDto());
        }

        public StudyDTO CreaStudyDto()
        {
            var teamDTO = new TeamDTO()
            {
                Id = 3
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



    }
}
