using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Storage.Repository;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class StorageController
    {
        public StudyStorageManager Study;
        public TeamStorageManager Team;
        public UserStorageManager User;
        public TaskStorageManager Task;

        public StorageController()
        {
            IGenericRepository repo = new EntityFrameworkGenericRepository<StudyContext>(new StudyContext());
            Study = new StudyStorageManager(repo);
            Team = new TeamStorageManager(repo);
            User = new UserStorageManager(repo);
            Task = new TaskStorageManager(repo);
        }

        public bool UpdateTeam(TeamDTO team)
        {
            //Find the existing team
            var teamToUpdate = Team.GetTeam(team.Id);
            //Make a new list for the new users
            var usersIDs = team.UserIDs;
            //clear the old users list
            teamToUpdate.Users.Clear();

            //Find each new user in the db  and add them 
            foreach (var user in usersIDs)
            {
                var u = User.GetUser(user);
                teamToUpdate.Users.Add(u);
            }

            teamToUpdate.Name = team.Name;
            teamToUpdate.Metadata = team.Metadata;
            return Team.UpdateTeam(teamToUpdate);
        }

        internal object GetTeam(int teamId)
        {
            throw new NotImplementedException();
        }
    }
}