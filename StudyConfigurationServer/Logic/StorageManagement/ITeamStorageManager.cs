#region Using

using System;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

#endregion

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public interface ITeamStorageManager
    {

        int CreateTeam(Team team);

        IQueryable<Team> GetAllTeams();

        bool RemoveTeam(int teamWithIdToDelete);
        bool UpdateTeam(Team team);
        Team GetTeam(int teamId);
        int SaveUser(User userToSave);

        bool RemoveUser(int userWithIdToDelete);
        bool UpdateUser(User user);
        IQueryable<User> GetAllUsers();
        User GetUser(int userId);
    }
}