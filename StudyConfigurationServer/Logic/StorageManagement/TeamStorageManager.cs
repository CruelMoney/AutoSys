using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using System.Linq;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class TeamStorageManager 
    {
        IGenericRepository _repo;
        public TeamStorageManager()
        {
            _repo = new EntityFrameworkGenericRepository<StudyContext>();
        }
        
        public TeamStorageManager(IGenericRepository repo)
        {
            _repo = repo;
        }

        public int CreateTeam(Team team)
        {
            return _repo.Create(team);
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return _repo.Read<Team>();
        }

        public bool RemoveTeam(int TeamWithIDToDelete)
        {
            return _repo.Delete(_repo.Read<Team>(TeamWithIDToDelete));
        }

        public bool UpdateTeam(Team team)
        {
            return _repo.Update(team);
        }
           
        public Team GetTeam(int TeamID)
        {
            return _repo.Read<Team>(TeamID);
        }

        public int SaveUser(User userToSave)
        {
            return _repo.Create(userToSave);
        }

        public bool RemoveUser(int userWithIdToDelete)
        {
            return _repo.Delete(_repo.Read<User>(userWithIdToDelete));
        }

        public bool UpdateUser(User user)
        {
           return _repo.Update(user);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _repo.Read<User>();
        }

        public User GetUser(int userId)
        {
            return _repo.Read<User>(userId);
        }


    }
   
}
