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
        private StorageController _storage;
        private TeamStorageManager _teamStorage;


        public TeamManager()
        {
            _storage = new StorageController();
           
        }

        public TeamManager(StorageController storage)
        {
            _storage = storage;
            _teamStorage = storage.Team;
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

            var users = new List<User>();
                foreach (var userID in teamDtoDto.UserIDs) {
                users.Add(_storage.User.GetUser(userID));
            }

              

            teamToAdd.Users = users;
            
            return _storage.Team.SaveTeam(teamToAdd);
        }

        public Boolean RemoveTeam(int TeamID)
        {
            return _storage.Team.RemoveTeam(TeamID);
        }

        public bool UpdateTeam(int teamId, TeamDTO team)
        {           
            var teamToUpdate = _storage.GetTeam(teamId);
             
            //Do logic checks on the team
                           
            return _storage.UpdateTeam(team);
        }

        public IEnumerable<TeamDTO> SearchTeams(string TeamName)
        {
            return
                 (from Team dbTeam in _storage.Team.GetAllTeams()
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
            var dbTeam = _storage.Team.GetTeam(teamId);
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
            var teams = _storage.Team.GetAllTeams();
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
                
                list.Add( new TeamDTO { Id = team.Id, Name = team.Name, UserIDs = team.Users.Select(u=>u.Id).ToArray() });
               
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

