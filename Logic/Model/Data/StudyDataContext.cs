using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model.Data
{
    public class StudyDataContext : DbContext, IDbContext
    {
        public DbSet<StudyLogic> Studies { get; set; }
        public DbSet<TeamLogic> Teams { get; set; }
        public DbSet<UserLogic> Users { get; set; }
        public DbSet<StageLogic> Stages { get; set; }
        public DbSet<CriteriaLogic> Criteria { get; set; }
        public DbSet<TaskLogic> Tasks { get; set; }
        public DbSet<ItemLogic> Items { get; set; }
    }
    
}
