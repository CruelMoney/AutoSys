#region Using

using System;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

#endregion

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class TeamStorageManager : ITeamStorageManager
    {
        private readonly IGenericRepository _repo;
        private readonly string _teamException = "Can't find team(s) in repository";
        private readonly string _userException = "Can't find user(s) in repository";

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

        public IQueryable<Team> GetAllTeams()
        {
            try
            {
                return _repo.Read<Team>();
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(_teamException);
            }
        }

        public bool RemoveTeam(int teamWithIdToDelete)
        {
            try
            {
                return _repo.Delete(_repo.Read<Team>(teamWithIdToDelete));
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(_teamException);
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
                throw new NullReferenceException(_teamException);
            }
        }

        public Team GetTeam(int teamId)
        {
            try
            {
                return _repo.Read<Team>(teamId);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(_teamException);
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
                throw new NullReferenceException(_userException);
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
                throw new NullReferenceException(_userException);
            }
        }

        public IQueryable<User> GetAllUsers()
        {
            try
            {
                return _repo.Read<User>();
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException(_userException);
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
                throw new NullReferenceException(_userException);
            }
        }
    }
}