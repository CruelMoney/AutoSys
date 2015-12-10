using System;
using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TeamCRUD
{
    public class TeamManager
    {
        private readonly TeamStorageManager _teamStorageManager;

        public TeamManager(TeamStorageManager storageManager)
        {
            _teamStorageManager = storageManager;
          

        }

        public TeamManager()
        {
           _teamStorageManager = new TeamStorageManager();
        }

        public int CreateTeam(TeamDTO teamDtoDto)
        {
            var teamToAdd = new Team()
            {
                Name = teamDtoDto.Name,
                Id = teamDtoDto.Id,
                Metadata = teamDtoDto.Metadata,
                UserIDs = teamDtoDto.UserIDs,
                Users = new List<User>()
            };

            foreach (var userID in teamDtoDto.UserIDs)
            {
                teamToAdd.Users.Add(_teamStorageManager.GetUser(userID));
            }

            return _teamStorageManager.CreateTeam(teamToAdd);
        }

        public Boolean RemoveTeam(int teamID)
        {
            var team = _teamStorageManager.GetTeam(teamID);

            //remove logic can team be removed if having study?

            return _teamStorageManager.RemoveTeam(teamID);
        }

        public bool UpdateTeam(int teamId, TeamDTO newTeamDto)
        {
            var teamToUpdate = _teamStorageManager.GetTeam(teamId);

            teamToUpdate.Name = newTeamDto.Name;

            foreach (var userID in newTeamDto.UserIDs)
            {
                teamToUpdate.Users.Add(_teamStorageManager.GetUser(userID));
            }

            return _teamStorageManager.UpdateTeam(teamToUpdate);
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
            var dbTeams = _teamStorageManager.GetAllTeams();
               
            foreach (var team in dbTeams)
            {
                var dbTeam = _teamStorageManager.GetTeam(team.Id);
                yield return new TeamDTO()
                  {
                      Id = dbTeam.Id,
                      Name = dbTeam.Name,
                      Metadata = dbTeam.Metadata,
                      UserIDs = dbTeam.Users.Select(u => u.Id).ToArray()
                };
            }
           
        }
    }
}

