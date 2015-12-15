#region Using

using System.Collections.Generic;
using System.Data.Entity;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser.bibTex;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServerTests.Properties;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests.DBInitializers
{
    public class StudyDb : DropCreateDatabaseAlways<StudyContext>
    {
        protected override void Seed(StudyContext context)
        {
            var testStudy = new Study();

            var testUser1 = new User {Name = "chris"};
            var testUser2 = new User {Name = "ramos"};
            var testUser3 = new User {Name = "kathrin"};
            var testUser4 = new User {Name = "emil"};
            var testUser5 = new User {Name = "user1"};
            var testUser6 = new User {Name = "user2"};
            var testUser7 = new User {Name = "user3"};
            var testUser8 = new User {Name = "user4"};

            context.Users.AddRange(new List<User> {testUser1, testUser2, testUser3, testUser4});

            var testTeam1 = new Team
            {
                Name = "team1",
                Users = new List<User> {testUser1, testUser2, testUser3, testUser4}
            };
            var testTeam2 = new Team
            {
                Name = "team2",
                Users = new List<User> {testUser5, testUser6, testUser7, testUser8}
            };
            var testTeam3 = new Team
            {
                Name = "team3",
                Users = new List<User> {testUser1, testUser6, testUser3, testUser8}
            };

            context.Teams.AddRange(new List<Team> {testTeam1, testTeam2, testTeam3});


            var expectedUserData1 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = "2000"}},
                UserId = 1
            };
            var expectedUserData2 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = "2015"}},
                UserId = 2
            };
            var expectedUserData3 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = "2015"}},
                UserId = 2
            };

            var emptyUserData1 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = null}},
                UserId = 1
            };
            var emptyUserData2 = new UserData
            {
                Data = new List<StoredString> {new StoredString {Value = null}},
                UserId = 2
            };


            //Finished
            var task1 = new StudyTask
            {
                DataFields = new List<DataField>
                {
                    new DataField
                    {
                        Name = "Year",
                        UserData = new List<UserData> {expectedUserData1, expectedUserData2}
                    }
                },
                Users = new List<User> {testUser1, testUser2},
                TaskType = StudyTask.Type.Review,
                IsEditable = true
            };

            //Unfinished
            var task2 = new StudyTask
            {
                DataFields = new List<DataField>
                {
                    new DataField
                    {
                        Name = "Year",
                        UserData = new List<UserData> {emptyUserData1, expectedUserData3}
                    }
                },
                Users = new List<User> {testUser1, testUser2},
                TaskType = StudyTask.Type.Review,
                IsEditable = true
            };

            //Unfinished
            var task3 = new StudyTask
            {
                DataFields = new List<DataField>
                {
                    new DataField
                    {
                        Name = "About",
                        UserData = new List<UserData> {emptyUserData2}
                    }
                },
                Users = new List<User> {testUser2},
                TaskType = StudyTask.Type.Review,
                IsEditable = true
            };


            var parser = new BibTexParser(new ItemValidator());
            var items = parser.Parse(Resources.bibtex.ToString());

            var testCriteria = new Criteria
            {
                DataType = DataField.DataType.String,
                Name = "Year",
                Rule = Criteria.CriteriaRule.LargerThan,
                DataMatch = new List<StoredString> {new StoredString {Value = "2000"}},
                Description = "expectedDescription",
                TypeInfo = new List<StoredString>()
            };

            var testCriteria2 = new Criteria
            {
                DataType = DataField.DataType.Boolean,
                Name = "About",
                Rule = Criteria.CriteriaRule.Equals,
                DataMatch = new List<StoredString> {new StoredString {Value = "true"}},
                Description = "expectedDescription2",
                TypeInfo = new List<StoredString>()
            };


            var testStage1 = new Stage
            {
                Tasks = new List<StudyTask> {task1, task2},
                IsCurrentStage = true,
                Name = "stage1",
                Criteria = new List<Criteria> {testCriteria},
                CurrentTaskType = StudyTask.Type.Review,
                DistributionRule = Stage.Distribution.HundredPercentOverlap
            };
            var testStage2 = new Stage
            {
                Tasks = new List<StudyTask> {task3},
                Name = "stage2",
                Criteria = new List<Criteria> {testCriteria2},
                CurrentTaskType = StudyTask.Type.Review,
                DistributionRule = Stage.Distribution.NoOverlap
            };


            testStudy = new Study
            {
                Name = "Test Study",
                IsFinished = false,
                Items = items,
                Team = testTeam1,
                Stages = new List<Stage> {testStage1, testStage2}
            };

            context.Studies.Add(testStudy);

            base.Seed(context);
        }
    }
}