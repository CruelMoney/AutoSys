using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using System.Linq;
using System;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class TeamStorageManager 
    {
        IGenericRepository _repo;
        string teamException = "Can't find team(s) in repository";
        string userException = "Can't find user(s) in repository";
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
            try
            {
                return _repo.Read<Team>();
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(teamException);
            }           
        }

        public bool RemoveTeam(int TeamWithIDToDelete)
        {
            try
            {
                return _repo.Delete(_repo.Read<Team>(TeamWithIDToDelete));
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(teamException);
            }
            
        }

        public bool UpdateTeam(Team team)
        {
            try
            {
                return _repo.Update(team);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(teamException);
            }
            
        }
           
        public Team GetTeam(int TeamID)
        {
            try
            {
                return _repo.Read<Team>(TeamID);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(teamException);
            }
            
        }

        public int SaveUser(User userToSave)
        {
            return _repo.Create(userToSave);
        }

        public bool RemoveUser(int userWithIdToDelete)
        {
            try
            {
                return _repo.Delete(_repo.Read<User>(userWithIdToDelete));
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(userException);
            }
           
        }

        public bool UpdateUser(User user)
        {
            try
            {
                return _repo.Update(user);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(userException);
            }
        }

        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return _repo.Read<User>();
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(userException);
            }
            
        }

        public User GetUser(int userId)
        {
            try
            {
                return _repo.Read<User>(userId);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(userException);
            }
            
        }


    }
   
}
