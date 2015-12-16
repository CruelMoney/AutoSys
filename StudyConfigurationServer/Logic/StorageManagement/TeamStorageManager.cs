#region Using

using System;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

#endregion

namespace StudyConfigurationServer.Logic.StorageManagement
{
    /// <summary>
    /// StorageManager responsible for saving, retrieving, updating and deleting teams
    /// </summary>
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

        /// <summary>
        /// Create a team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public int CreateTeam(Team team)
        {
            return _repo.Create(team);
        }

        /// <summary>
        /// Retrieve all teams
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Delete a team
        /// </summary>
        /// <param name="teamWithIdToDelete"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update a team
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get a single team
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Store a user in the database
        /// </summary>
        /// <param name="userToSave"></param>
        /// <returns></returns>
        public int SaveUser(User userToSave)
        {
            return _repo.Create(userToSave);
        }

        /// <summary>
        /// Delete a user from the database
        /// </summary>
        /// <param name="userWithIdToDelete"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve all users
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Get a single user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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