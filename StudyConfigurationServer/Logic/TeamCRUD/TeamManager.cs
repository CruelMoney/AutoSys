#region Using

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

#endregion

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

        public int CreateTeam(TeamDto teamDto)
        {

            var teamToAdd = new Team
            {
                Name = teamDto.Name,
                ID = teamDto.Id,
                Metadata = teamDto.Metadata,
                Users = new List<User>()
            };

            foreach (var userId in teamDto.UserIDs)
            {
                if (_teamStorageManager.GetUser(userId) == null)
                {
                    throw new NullReferenceException("User doesn't exist in database");
                }
            }
           
            foreach (var userId in teamDto.UserIDs)
            {
                teamToAdd.Users.Add(_teamStorageManager.GetUser(userId));
            }

            return _teamStorageManager.CreateTeam(teamToAdd);
        }

        public bool RemoveTeam(int teamId)
        {
            try
            {
                var team = _teamStorageManager.GetAllTeams()
                    .Where(t => t.ID == teamId)
                    .Include(t => t.Studies)
                    .FirstOrDefault();

                if (team.Studies.Any())
                {
                    throw new ArgumentException("Can't delete team because it is in a study");
                }
                return _teamStorageManager.RemoveTeam(teamId);
            }
            catch (NullReferenceException)
            {
                throw new NullReferenceException("Can't find team in database");
            }
        }

        public bool UpdateTeam(int teamId, TeamDto newTeamDto)
        {
            try
            {
                var teamToUpdate = _teamStorageManager.GetTeam(teamId);
                if (teamToUpdate == null)
                {
                    throw new NullReferenceException("Team Doesn't exist in database");
                }
                if (newTeamDto.UserIDs.Length == 0)
                {
                    throw new ArgumentException("You can't add or delete users from a team, only change its name");
                }
                if (!teamToUpdate.Users.Any())
                {
                    throw new ArgumentException("Team can't exist without users");
                }
                var teamToUpdateArray = teamToUpdate.Users.Select(u => u.ID).ToArray();
                var newTeamArray = newTeamDto.UserIDs;
                for (var i = 0; i < teamToUpdate.Users.Count; i++)
                {
                    if (teamToUpdateArray[i] == newTeamArray[i])
                    {
                    }
                    else
                    {
                        throw new ArgumentException("You can't add or delete users from a team, only change its name");
                    }
                }


                teamToUpdate.Users.Clear();
                teamToUpdate.Name = newTeamDto.Name;

                foreach (var userId in newTeamDto.UserIDs)
                {
                    try
                    {
                        teamToUpdate.Users.Add(_teamStorageManager.GetUser(userId));
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

        public IEnumerable<TeamDto> SearchTeamDtOs(string teamName)
        {
            try
            {
                return
                    (from Team dbTeam in _teamStorageManager.GetAllTeams()
                        where dbTeam.Name.Equals(teamName)
                        select new TeamDto
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

        public TeamDto GetTeamDto(int teamId)
        {
            try
            {
                var dbTeam = _teamStorageManager.GetTeam(teamId);
                return new TeamDto
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


        public IEnumerable<TeamDto> GetAllTeamDtOs()
        {
            try
            {
                var dbTeams = _teamStorageManager.GetAllTeams();
                var list = new List<TeamDto>();
                foreach (var team in dbTeams)
                {
                    list.Add(new TeamDto
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