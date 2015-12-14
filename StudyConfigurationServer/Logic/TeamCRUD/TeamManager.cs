using System;
using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;
using System.Data.Entity;

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
            foreach(var userId in teamDtoDto.UserIDs)
            {
                if (_teamStorageManager.GetUser(userId) == null)
                {
                    throw new NullReferenceException("User doesn't exist in database");
                }
            }
            var teamToAdd = new Team()
            {
                Name = teamDtoDto.Name,
                ID = teamDtoDto.Id,
                Metadata = teamDtoDto.Metadata,
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
            try
            {
                var team = _teamStorageManager.GetAllTeams()
                    .Where(t => t.ID == teamID).
                    Include(t => t.Studies)
                    .FirstOrDefault();

                if (team.Studies.Any())
                {
                    throw new ArgumentException("Can't delete team because it is in a study");
                }
                return _teamStorageManager.RemoveTeam(teamID);

            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Can't find team in database");
            }       
        }

        public bool UpdateTeam(int teamId, TeamDTO newTeamDto)
        {
            try
            {
                
                var teamToUpdate = _teamStorageManager.GetTeam(teamId);
                if(teamToUpdate == null) { throw new NullReferenceException("Team Doesn't exist in database");}
                if (newTeamDto.UserIDs.Length ==0) { throw new ArgumentException("You can't add or delete users from a team, only change its name"); }
                if (!teamToUpdate.Users.Any()) { throw new ArgumentException("Team can't exist without users"); }
                var teamToUpdateArray = teamToUpdate.Users.Select(u => u.ID).ToArray();
                var newTeamArray = newTeamDto.UserIDs;
                for (int i = 0; i < teamToUpdate.Users.Count; i++)
                {
                    if (teamToUpdateArray[i] == newTeamArray[i])
                    {
                        continue;
                    }
                    else { throw new ArgumentException("You can't add or delete users from a team, only change its name"); }
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
            catch (NullReferenceException)
            {
                throw new NullReferenceException("team does not exist");
            }
                       
        }

        public IEnumerable<TeamDTO> SearchTeamDTOs(string TeamName)
        {
            
            try
            {
              return
                     (from Team dbTeam in _teamStorageManager.GetAllTeams()
                      where dbTeam.Name.Equals(TeamName)
                      select new TeamDTO()
                      {
                          Id = dbTeam.ID,
                          Name = dbTeam.Name,
                          Metadata = dbTeam.Metadata,
                          UserIDs = dbTeam.Users.Select(u => u.ID).ToArray()
                      }).ToList();
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("No teams in database, no reason to search my friend");
            }
        }

        public TeamDTO GetTeamDTO(int teamId)
        {
            try
            {
                var dbTeam = _teamStorageManager.GetTeam(teamId);
                return new TeamDTO()
                {
                    Id = dbTeam.ID,
                    Name = dbTeam.Name,
                    Metadata = dbTeam.Metadata,
                    UserIDs = dbTeam.Users.Select(u => u.ID).ToArray()
                };
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("team could not be found, probably does not exist in database");
            }
        }

        public Team GetTeam(int teamId)
        {
            try
            {
                return _teamStorageManager.GetTeam(teamId);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Team could not be found, probably does not exist in database");
            }
            
        }

        

        public IEnumerable<TeamDTO> GetAllTeamDTOs()
        {
            try
            {
                var dbTeams = _teamStorageManager.GetAllTeams();
                var list = new List<TeamDTO>();
                foreach (var team in dbTeams)
                {
                    list.Add(new TeamDTO()
                    {
                        Id = team.ID,
                        Name = team.Name,
                        Metadata = team.Metadata,
                        UserIDs = team.Users.Select(u => u.ID).ToArray()
                    });
                }
                return list;
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("teams could not be found, probably doesn't exist in database");
            }
           
        }
    }
}

