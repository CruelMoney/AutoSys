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
            _teamRepo = new EntityFrameworkRepository<StudyDataContext>();
        }
        
        public TeamStorageManager(IRepository repo)
        {
            _teamRepo = repo;
        }

        public void SaveTeam(Team TeamToSave)
        {
            var TeamLogicToSave = new TeamLogic(TeamToSave);
            _teamRepo.Create(TeamLogicToSave);
        }

        public void RemoveTeam(int TeamWithIDToDelete)
        {
            _teamRepo.Delete(_teamRepo.Read<TeamLogic>(TeamWithIDToDelete));
        }

        public void UpdateTeam(Team TeamToUpdate)
        { 
            var TeamLogicToUpdate = new TeamLogic(TeamToUpdate);
            _teamRepo.Update<TeamLogic>(TeamLogicToUpdate);
        }

        public IEnumerable<TeamLogic> SearchTeams(String TeamName)
        {
            foreach(TeamLogic t in (_teamRepo.Read<TeamLogic>()))
                if (t.Name.Contains(TeamName.ToLower()))
                {
                    yield return t ;
                }
            yield break;
        }
            
        

        public TeamLogic GetTeam(int TeamID)
        {
            return _teamRepo.Read<TeamLogic>(TeamID);
        }

    }
   
}
