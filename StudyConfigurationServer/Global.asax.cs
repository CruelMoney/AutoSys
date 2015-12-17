using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using StudyConfigurationServer.Logic.StudyManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        TaskSubmissionDto _task;
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            Database.SetInitializer(new MultipleTeamsDb());
            var context = new StudyContext();
            context.Database.Initialize(true);

            var studyManager = new StudyManager();

            studyManager.CreateStudy(CreaStudyDto());
        }

        internal class MultipleTeamsDb : DropCreateDatabaseAlways<StudyContext>
        {
            protected override void Seed(StudyContext context)
            {

                //Here it is possible to initialize the db with a custom context
                var testUser1 = new User() { ID = 1, Name = "chris", };
                var testUser2 = new User() { ID = 2, Name = "ramos" };
                var testUser3 = new User() { ID = 3, Name = "kathrin" };
                var testUser4 = new User() { ID = 4, Name = "emil" };
                var testUser5 = new User() { ID = 5, Name = "user1" };
                var testUser6 = new User() { ID = 6, Name = "user2" };
                var testUser7 = new User() { ID = 7, Name = "user3" };
                var testUser8 = new User() { ID = 8, Name = "user4" };

                context.Users.AddRange(new List<User>() { testUser1, testUser2, testUser3, testUser4 });

                var testTeam1 = new Team() { Name = "team1", Users = new List<User>() { testUser1, testUser2, testUser3, testUser4 } };
                var testTeam2 = new Team() { Name = "team2", Users = new List<User>() { testUser5, testUser6, testUser7, testUser8 } };
                var testTeam3 = new Team() { Name = "team3", Users = new List<User>() { testUser1, testUser6, testUser3, testUser8 } };

                context.Teams.AddRange(new List<Team>() { testTeam1, testTeam2, testTeam3 });

                base.Seed(context);
            }
        }


        public StudyDto CreaStudyDto()
        {
            var teamDto = new TeamDto()
            {
                Id = 1
            };

            var criteria1 = new CriteriaDto()
            {
                Name = "Year",
                Rule = CriteriaDto.CriteriaRule.LargerThan,
                DataMatch = new string[] { "2001" },
                DataType = DataFieldDto.DataType.String,
                Description = "Write the year of the study",
            };

            var criteria2 = new CriteriaDto()
            {
                Name = "About Snails",
                DataType = DataFieldDto.DataType.Boolean,
                Rule = CriteriaDto.CriteriaRule.Equals,
                DataMatch = new string[] { "true" },
                Description = "Write true the study is about snails, and false if it is not.",
            };

            var stage1 = new StageDto()
            {
                Name = "stage1",
                Criteria = criteria1,
                DistributionRule = StageDto.Distribution.HundredPercentOverlap,
                ReviewerIDs = new int[] { 1, 2 },
                ValidatorIDs = new int[] { 3 },
                VisibleFields = new StageDto.FieldType[] { StageDto.FieldType.Title, StageDto.FieldType.Author, StageDto.FieldType.Year },

            };

            var stage2 = new StageDto()
            {
                Name = "stage2",
                Criteria = criteria2,
                DistributionRule = StageDto.Distribution.HundredPercentOverlap,
                ReviewerIDs = new int[] { 3, 2 },
                ValidatorIDs = new int[] { 4, 1 },
                VisibleFields = new StageDto.FieldType[] { StageDto.FieldType.Title, StageDto.FieldType.Author, StageDto.FieldType.Year }
            };

            var studyDto = new StudyDto()
            {
                Name = "testStudy",
                Team = teamDto,
                Items = Properties.Resources.bibtex_small,
                Stages = new StageDto[] { stage1, stage2 }
            };

            return studyDto;
        }
    

    }
}
