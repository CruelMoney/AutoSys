using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model;
using Logic.Model.DTO;
using Storage.Repository;

namespace Logic.StorageManagement
{
    public class TeamStorageManager 
    {
        IRepository _teamRepo;
        public TeamStorageManager()
        {
        }
        
        public TeamStorageManager(IRepository repo)
        {
            _teamRepo = repo;
        }
        public void SaveTeam(Team TeamToSave)
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
