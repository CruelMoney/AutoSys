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
            //var StoredTeamToSave = new StoredTeam(TeamToSave);
            //_teamRepo.Create<StoredTeam>(StoredTeamToSave);
        }

        public void RemoveTeam(int TeamWithIDToDelete)
        {
            throw new NotImplementedException();
            //var StoredTeamToDelete = new StoredTeam(TeamToDelete);
            _teamRepo.Delete<StoredTeam>(_teamRepo.Read<StoredTeam>(TeamWithIDToDelete));
        }

        public void UpdateTeam(int TeamID, String UpdatedName, String UpdatedMetaData)
        {
            throw new NotImplementedException();
            //var StoredTeamToUpdate = new StoredTeam(TeamToUpdate);
            //_teamRepo.Update<StoredTeam>(StoredTeamToUpdate);
        }

        public IEnumerable<Team> SearchTeams(String TeamName)
        {
            throw new NotImplementedException();
            //foreach(StoredTeam t in (_teamRepo.Read<StoredTeam>()){
            //    yield t.Name.equals(TeamName);                        
            //}
        }

        public Team GetTeam(int TeamID)
        {
            throw new NotImplementedException();
            _teamRepo.Read<StoredTeam>(TeamID);
        }

    }
   
}
