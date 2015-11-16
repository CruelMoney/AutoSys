using System;
using System.Collections.Generic;
using Storage.Repository;

namespace Logic.Model
{
    public class Study : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Team AssociatedTeam { get; set; }
        public List<Phase> StudyPhases { get; set; } 
        public List<Criteria>  StudyCriteria { get; set; }
        public List<Item> StudyData { get; set; }
        public Dictionary<User, User.Role> Users { get; set; }

        public void addPhase()
        {
            throw new NotImplementedException();
        }

        public void addCriteria()
        {
            throw new NotImplementedException();
        }
    }

}