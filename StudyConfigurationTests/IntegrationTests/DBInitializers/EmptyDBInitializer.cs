#region Using

using System.Data.Entity;
using StudyConfigurationServer.Models.Data;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests.DBInitializers
{
    public class EmptyDbInitializer : DropCreateDatabaseAlways<StudyContext>
    {
        protected override void Seed(StudyContext context)
        {
            //Here it is possible to initialize the db with a custom context
            //context.Tasks.AddOrUpdate();

            base.Seed(context);
        }
    }
}