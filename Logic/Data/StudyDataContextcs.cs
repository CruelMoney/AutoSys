using System.Data.Entity;
using Logic.Data.StoredData;
using Storage.Repository;

namespace Logic.Data
{
    //TODO we have to figure out we store the things 
    public class StudyDataContextcs : DbContext, IDbContext
    {
        public DbSet<StoredStudy> Studies { get; set; }
        public DbSet<StoredTeam> Teams { get; set; }
        public DbSet<StoredUser> Users { get; set; }
        public DbSet<StoredPhase> Phases { get; set; }
        public DbSet<StoredTask> Tasks { get; set; }
        public DbSet<StoredCriteria> Criterias { get; set; }
        public DbSet<StoredItem> Items { get; set; }


    }
}
