using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.Repository;

namespace Logic.Data
{
    public class Team : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<User> TeamList { get; set; }

        public void AddUser(User userToAdd)
        {
            TeamList.Add(userToAdd);
        }

        public void RemoveUser(int userToRemoveID)
        {
            throw new NotImplementedException();
        }
    }
}
