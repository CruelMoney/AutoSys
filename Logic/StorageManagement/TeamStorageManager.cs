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

        public int SaveTeam(Team TeamToSave)
        {
            return _teamRepo.Create(TeamToSave);
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return _teamRepo.Read<Team>();
        }

        public bool RemoveTeam(int TeamWithIDToDelete)
        {
            return _teamRepo.Delete(_teamRepo.Read<Team>(TeamWithIDToDelete));
        }

        public bool UpdateTeam(Team TeamToUpdate)
        {
            return _teamRepo.Update(TeamToUpdate);
        }

        public IEnumerable<Team> SearchTeams(string TeamName)
        {
            return _teamRepo.Read<Team>();
        }
           

        public Team GetTeam(int TeamID)
        {
            return _teamRepo.Read<Team>(TeamID);
        }

    }
   
}
