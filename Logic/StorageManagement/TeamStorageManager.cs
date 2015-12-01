using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model;
using Logic.Model.Data;
using Logic.Model.DTO;
using Storage.Repository;
using Logic.Model.Data;

namespace Logic.StorageManagement
{
    public class TeamStorageManager 
    {
        IGenericRepository _teamRepo;
        public TeamStorageManager()
        {
            _teamRepo = new EntityFrameworkGenericRepository<StudyDataContext>();
        }
        
        public TeamStorageManager(IGenericRepository repo)
        {
            _teamRepo = repo;
        }

        public int SaveTeam(TeamLogic TeamToSave)
        {
            return _teamRepo.Create(TeamToSave);
        }

        public IEnumerable<TeamLogic> GetAllTeams()
        {
            return _teamRepo.Read<TeamLogic>();
        }

        public bool RemoveTeam(int TeamWithIDToDelete)
        {
            return _teamRepo.Delete(_teamRepo.Read<TeamLogic>(TeamWithIDToDelete));
        }

        public bool UpdateTeam(TeamLogic TeamToUpdate)
        {
            return _teamRepo.Update(TeamToUpdate);
        }

        public IEnumerable<TeamLogic> SearchTeams(string TeamName)
        {
            return _teamRepo.Read<TeamLogic>();
        }
            
        

        public TeamLogic GetTeam(int TeamID)
        {
            return _teamRepo.Read<TeamLogic>(TeamID);
        }

    }
   
}
