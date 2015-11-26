using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model;
using Logic.Model.DTO;
using Storage.Repository;
using Logic.Model.Data;

namespace Logic.StorageManagement
{
    public class TeamStorageManager 
    {
        IRepository _teamRepo;
        public TeamStorageManager()
        {
            _teamRepo = new EntityFrameworkRepository<TContext>();
        }
        
        public TeamStorageManager(IRepository repo)
        {
            _teamRepo = repo;
        }

        public void SaveTeam(Team TeamToSave)
        {
            var StoredTeamToSave = new StoredTeam(TeamToSave);
            _teamRepo.Create(StoredTeamToSave);
        }

        public void RemoveTeam(int TeamWithIDToDelete)
        {
            _teamRepo.Delete(_teamRepo.Read<StoredTeam>(TeamWithIDToDelete));
        }

        public void UpdateTeam(Team TeamToUpdate)
        { 
            var StoredTeamToUpdate = new StoredTeam(TeamToUpdate);
            _teamRepo.Update<StoredTeam>(StoredTeamToUpdate);
        }

        public IEnumerable<Team> SearchTeams(String TeamName)
        {
            foreach(StoredTeam t in (_teamRepo.Read<StoredTeam>()))
                if (t.Name.Contains(TeamName.ToLower()))
                {
                    yield return new Team(t);
                }
            yield break;
        }
            
        

        public Team GetTeam(int TeamID)
        {
            return new Team(_teamRepo.Read<StoredTeam>(TeamID));
        }

    }
   
}
