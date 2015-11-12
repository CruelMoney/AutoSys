using System.Data.Entity;
using Storage.Repository;
using System;

namespace Logic.Data
{
    public class Study : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Team AssociatedTeam { get; set; }

        public void SetInclusionCriteria() //Idea for method
        {
            throw new NotImplementedException();
        }

        public void SetExclusionCriteria() //Idea for method
        {
            throw new NotImplementedException();
        }
    }

    


    public class StudyContext : DbContext
    {
        public DbSet<Study> studies { get; set; }
    }

}