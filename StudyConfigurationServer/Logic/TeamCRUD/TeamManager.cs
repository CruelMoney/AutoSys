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
                try
                {
                teamToAdd.Users.Add(_teamStorageManager.GetUser(userID));
            }
                catch (NullReferenceException)
                {
                    throw new NullReferenceException("User can't be added to team, because user does not exist");
                }

            }

            return _teamStorageManager.CreateTeam(teamToAdd);
        }

        public Boolean RemoveTeam(int teamID)
        {

            var team = _teamStorageManager.GetTeam(teamID);
            if (team.StudyIDs != null)
            {
            return _teamStorageManager.RemoveTeam(teamID);
        }
            else
            {
                throw new ArgumentException("Can't delete team because it is in a study");
            }
        }

        public bool UpdateTeam(int teamId, TeamDTO newTeamDto)
        {
            var teamToUpdate = _teamStorageManager.GetTeam(teamId);
            if(teamToUpdate == null)
            {
                throw new NullReferenceException("team does not exist");
            }
            foreach(var userId in newTeamDto.UserIDs)
            {
                if (teamToUpdate.UserIDs.Contains(userId))
                {
                    continue;
                }
                else
                {
                    throw new ArgumentException("You can't add or delete users from a team, only change its name, you dipwit");
                }
            }

            teamToUpdate.Users.Clear();

            teamToUpdate.Name = newTeamDto.Name;


            foreach (var userID in newTeamDto.UserIDs)
            {
                try
                {
                teamToUpdate.Users.Add(_teamStorageManager.GetUser(userID));
            }
                catch (NullReferenceException)
                {
                    throw new NullReferenceException("User can't be added to team, because user does not exist");
                }                    
            }           
            return _teamStorageManager.UpdateTeam(teamToUpdate);
        }

        public IEnumerable<TeamDTO> SearchTeams(string TeamName)
        {
            if (_teamStorageManager.GetAllTeams() == null)
            {
                throw new NullReferenceException("No teams in database, no reason to search my friend");
            }
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
            if(dbTeam == null)
            {
                throw new NullReferenceException("team could not be found, probably does not exist in database");
            }
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
               
            if(dbTeams == null)
            {
                throw new NullReferenceException("teams could not be found, probably doesn't exist in database");
            }
               
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

