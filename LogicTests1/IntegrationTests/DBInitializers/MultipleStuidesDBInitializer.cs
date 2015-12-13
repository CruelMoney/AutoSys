using System;
using System.Collections.Generic;
using System.Data.Entity;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace LogicTests1.IntegrationTests.DBInitializers
{
    public class MultipleStuidesDBInitializer : DropCreateDatabaseAlways<StudyContext>
    {
        protected override void Seed(StudyContext context)
        {

            //Here it is possible to initialize the db with a custom context

            var testUser1 = new User() { ID = 1, Name = "chris" };
            var testUser2 = new User() { ID = 2, Name = "ramos" };

            context.Users.AddRange(new List<User>() {testUser1, testUser2});

        

            base.Seed(context);
        }

    }
}
