using System;
using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TeamCRUD
{
    public class TeamManager
    {
        private readonly TeamStorageManager _teamStorageManager;
        private readonly UserStorageManager _userStorageManager;

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

        public int CreateTeam(TeamDTO teamDtoDto)
        {
            var teamToAdd = new Team()
            {
                Name = teamDtoDto.Name,
                Id = teamDtoDto.Id,
                Metadata = teamDtoDto.Metadata,
                UserIDs = teamDtoDto.UserIDs
            };

            var users = (from User dbUser in _userStorageManager.GetAllUsers()
                         where dbUser.Id.Equals(teamDtoDto.UserIDs)
                         select dbUser).ToList();

            teamToAdd.Users = users;
            
            return _teamStorageManager.SaveTeam(teamToAdd);
        }

        public Boolean RemoveTeam(int TeamID)
        {
            return _teamStorageManager.RemoveTeam(TeamID);
        }

        public bool UpdateTeam(int teamId, TeamDTO newTeamDto)
        {
            var updatedTeam = new Team()
            {
                Id = teamId,
                Name = newTeamDto.Name,
                Metadata = newTeamDto.Metadata,
            };

            var users = (from User dbUser in _userStorageManager.GetAllUsers()
                         where dbUser.Id.Equals(newTeamDto.UserIDs)
                         select dbUser).ToList();

            updatedTeam.Users = users;

            return _teamStorageManager.UpdateTeam(updatedTeam);
        }

        public IEnumerable<TeamDTO> SearchTeams(string TeamName)
        {
            return
                 (from Team dbTeam in _teamStorageManager.GetAllTeams()
                  where dbTeam.Name.Equals(TeamName)
                  select new TeamDTO()
                  {
                      Id = dbTeam.Id,
                      Name = dbTeam.Name,
                      Metadata = dbTeam.Metadata,
                      UserIDs = dbTeam.Users.Select(u=>u.Id).ToArray()
                  }).ToList();

        }

        public TeamDTO GetTeam(int teamId)
        {
            var dbTeam = _teamStorageManager.GetTeam(teamId);
            return new TeamDTO()
            {
                Id = dbTeam.Id,
                Name = dbTeam.Name,
                Metadata = dbTeam.Metadata,
                UserIDs = dbTeam.Users.Select(u => u.Id).ToArray()
            };
        }

        public IEnumerable<TeamDTO> GetAllTeams()
        {
            var teams = _teamStorageManager.GetAllTeams();
            int i = 0;
            var list = new List<TeamDTO>();
            foreach(var team in teams)
            {
                /*
                var userIds = new int[team.Users.Count];
                foreach (var user in team.Users)
                {
                    
                    var userId = user.Id;
                    userIds[i++] = user.Id;

                }
                i = 0;*/
                
                list.Add( new TeamDTO { Id = team.Id, Name = team.Name, UserIDs = team.UserIDs}); // team.Users.Select(u=>u.Id).ToArray() };
               
            } return list;
           /* return
                 (from Team dbTeam in _teamStorageManager.GetAllTeams()
                  select new TeamDTO()
                  {
                      Id = dbTeam.Id,
                      Name = dbTeam.Name,
                      Metadata = dbTeam.Metadata,
                      UserIDs = dbTeam.Users.Select(u => u.Id).ToArray()
                  }).ToList();*/
        }

    }
}

