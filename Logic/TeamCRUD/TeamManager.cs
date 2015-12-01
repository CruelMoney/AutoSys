using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.StorageManagement;
using Logic.Model.DTO;
using Logic.Model;

namespace Logic.TeamCRUD
{
    public class TeamManager
    {
        TeamStorageManager _teamStorageManager;
        UserStorageManager _userStorageManager;

        public TeamManager()
        {
            _teamStorageManager = new TeamStorageManager();
            _userStorageManager = new UserStorageManager();
        }

        public TeamManager(TeamStorageManager storageManager, UserStorageManager userStorageManager)
        {
            _teamStorageManager = storageManager;
            _userStorageManager = userStorageManager;
        }

        public int CreateTeam(Team teamDTO)
        {
            var teamToAdd = new TeamLogic()
            {
                Name = teamDTO.Name,
                Id = teamDTO.Id,
                Metadata = teamDTO.Metadata,
            };

            var users = (from UserLogic dbUser in _userStorageManager.GetAllUsers()
                         where dbUser.Id.Equals(teamDTO.UserIDs)
                         select dbUser).ToList();

            teamToAdd.Users = users;
            
            return _teamStorageManager.SaveTeam(teamToAdd);
        }

        public void RemoveTeam(int TeamID)
        {
            _teamStorageManager.RemoveTeam(TeamID);
        }

        public bool UpdateTeam(int teamId, Team newTeam)
        {
            var updatedTeam = new TeamLogic()
            {
                Id = teamId,
                Name = newTeam.Name,
                Metadata = newTeam.Metadata,
            };

            var users = (from UserLogic dbUser in _userStorageManager.GetAllUsers()
                         where dbUser.Id.Equals(newTeam.UserIDs)
                         select dbUser).ToList();

            updatedTeam.Users = users;

            return _teamStorageManager.UpdateTeam(updatedTeam);
        }

        public IEnumerable<Team> SearchTeams(string TeamName)
        {
            return
                 (from TeamLogic dbTeam in _teamStorageManager.GetAllTeams()
                  where dbTeam.Name.Equals(TeamName)
                  select new Team()
                  {
                      Id = dbTeam.Id,
                      Name = dbTeam.Name,
                      Metadata = dbTeam.Metadata,
                      UserIDs = dbTeam.Users.Select(u=>u.Id).ToArray()
                  }).ToList();

        }

        public Team GetTeam(int teamId)
        {
            var dbTeam = _teamStorageManager.GetTeam(teamId);
            return new Team()
            {
                Id = dbTeam.Id,
                Name = dbTeam.Name,
                Metadata = dbTeam.Metadata,
                UserIDs = dbTeam.Users.Select(u => u.Id).ToArray()
            };
        }

    }
}

