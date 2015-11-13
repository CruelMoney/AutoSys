using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Data.StoredData;
using Storage.Repository;

namespace Logic.Data
{
    public class TeamDataContext:DbContext,IDbContext
    {
        public DbSet<StoredTeam> Teams { get; set; }
        public DbSet<StoredUser> Users { get; set; }
  
    }
}
