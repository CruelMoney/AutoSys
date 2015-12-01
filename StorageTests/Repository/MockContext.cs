using System.Data.Entity;
using Storage.Repository;

namespace StorageTests.Repository
{
    public class MockContext : DbContext, IDbContext
    {
        public DbSet<MockEntity> Studies { get; set; }
    }
}
