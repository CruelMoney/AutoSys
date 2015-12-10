using System.Data.Entity;
using System.Data.Entity.Migrations;
using StudyConfigurationServer.Models.Data;

namespace LogicTests1.IntegrationTests.DBInitializers
{
    public class EmptyDBInitializer : DropCreateDatabaseAlways<StudyContext>
    {
        protected override void Seed(StudyContext context)
        {

            //Here it is possible to initialize the db with a custom context
            //context.Tasks.AddOrUpdate();

            base.Seed(context);
        }

    }
}
