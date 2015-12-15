#region Using

using System.Data.Entity;
using Storage.Repository;

#endregion

namespace StorageTests.Repository
{
    public class MockContext : DbContext, IDbContext
    {
        public DbSet<MockEntity> Studies { get; set; }
    }
}