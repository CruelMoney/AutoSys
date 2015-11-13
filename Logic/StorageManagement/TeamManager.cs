using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Data;
using Storage.Repository;

namespace Logic.StorageManagement
{
    public class TeamManager 
    {
        IRepository _teamRepo;
        public TeamManager()
        {
        }
        
        public TeamManager(IRepository repo)
        {
            _teamRepo = repo;
        }
        public void CreateTeam(String TeamName, IEnumerable<User> UserList, String MetaData)
        {
            throw new NotImplementedException();
        }

        public void RemoveTeam(int TeamID)
        {
            throw new NotImplementedException();
        }

        public void UpdateTeam(int TeamID, String UpdatedName, String UpdatedMetaData)
        {
            throw new NotImplementedException();
        }

        public Boolean TeamExistsByName(String TeamName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Team> SearchTeams(String TeamName)
        {
            throw new NotImplementedException();
        }

        public Team GetTeam(int TeamID)
        {
            throw new NotImplementedException();
        }

    }
   
}
