#region Using

using System.Collections.Generic;
using System.Data.Entity;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests.DBInitializers
{
    public class MultipleTeamsDb : DropCreateDatabaseAlways<StudyContext>
    {
        protected override void Seed(StudyContext context)
        {
            //Here it is possible to initialize the db with a custom context

            var testUser1 = new User {ID = 1, Name = "chris"};
            var testUser2 = new User {ID = 2, Name = "ramos"};
            var testUser3 = new User {ID = 3, Name = "kathrin"};
            var testUser4 = new User {ID = 4, Name = "emil"};
            var testUser5 = new User {ID = 5, Name = "user1"};
            var testUser6 = new User {ID = 6, Name = "user2"};
            var testUser7 = new User {ID = 7, Name = "user3"};
            var testUser8 = new User {ID = 8, Name = "user4"};

            context.Users.AddRange(new List<User> {testUser1, testUser2, testUser3, testUser4});

            var testTeam1 = new Team
            {
                ID = 1,
                Name = "team1",
                Users = new List<User> {testUser1, testUser2, testUser3, testUser4}
            };
            var testTeam2 = new Team
            {
                ID = 2,
                Name = "team2",
                Users = new List<User> {testUser5, testUser6, testUser7, testUser8}
            };
            var testTeam3 = new Team
            {
                ID = 3,
                Name = "team3",
                Users = new List<User> {testUser1, testUser6, testUser3, testUser8}
            };

            context.Teams.AddRange(new List<Team> {testTeam1, testTeam2, testTeam3});

            base.Seed(context);
        }
    }
}