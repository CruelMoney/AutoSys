using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace LogicTests1.IntegrationTests.DBInitializers
{
    public class MultipleTeamsDB : DropCreateDatabaseAlways<StudyContext>
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

            var testTeam1 = new Team() {Name = "team1" ,Users = new List<User>() {testUser1,testUser2, testUser3, testUser4} };
            var testTeam2 = new Team() { Name = "team2", Users = new List<User>() { testUser5, testUser6, testUser7, testUser8 } };
            var testTeam3 = new Team() { Name = "team3", Users = new List<User>() { testUser1, testUser6, testUser3, testUser8 } };

            context.Teams.AddRange(new List<Team>() {testTeam1, testTeam2, testTeam3});

            base.Seed(context);
        }

    }
}
