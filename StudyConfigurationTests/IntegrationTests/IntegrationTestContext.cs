﻿#region Using

using System.Data.Entity;
using Storage.Repository;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServerTests.IntegrationTests
{
    public class IntegrationTestContext : DbContext, IDbContext
    {
        public DbSet<UserStudies> UserStudies { get; set; }
        public DbSet<Study> Studies { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Criteria> Criteria { get; set; }
        public DbSet<StudyTask> Tasks { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<DataField> DataFields { get; set; }
        public DbSet<UserData> EnteredData { get; set; }
    }
}