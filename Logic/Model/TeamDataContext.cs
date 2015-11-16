using System.Data.Entity;
using Logic.Model.StoredData;
using Storage.Repository;

namespace Logic.Model
{
    public class TeamDataContext:DbContext,IDbContext
    {
        public DbSet<StoredTeam> Teams { get; set; }
        public DbSet<StoredUser> Users { get; set; }
  
    }
}
