using System.Data.Entity;
using Storage.Repository;

namespace Logic.Model.Data
{
    public class TeamDataContext:DbContext,IDbContext
    {
        public DbSet<StoredTeam> Teams { get; set; }
        public DbSet<StoredUser> Users { get; set; }
  
    }
}
