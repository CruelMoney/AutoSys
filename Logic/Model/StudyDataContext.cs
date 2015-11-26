using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Model
{
    public class StudyDataContext : DbContext, IDbContext
    {
        public DbSet<Study> Studies { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Criteria> Criteria { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Item> Items { get; set; }
    }
    
}
