#region Using

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServer.Logic.TeamUserManagement
{
    /// <summary>
    /// A manager class responsible for team logic
    /// </summary>
    public class TeamManager : ITeamManager
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

        /// <summary>
        /// Convert a TeamDTO and store it in the repositoy
        /// </summary>
        /// <param name="teamDto"> DTO of the team to be created</param>
        /// <returns></returns>
        public int CreateTeam(TeamDto teamDto)
        {

            if (teamDto.UserIDs == null || teamDto.UserIDs.Length == 0)
            {
                throw new ArgumentException("The team doesn't have any users");
            }

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

        /// <summary>
        /// Remove a team from the repository
        /// </summary>
        /// <param name="teamId"> Id of the team to be removed</param>
        /// <returns></returns>
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

        /// <summary>
        /// Update a team in the repository  with a new teamDTO
        /// Convert the teamDTO to a team
        /// </summary>
        /// <param name="teamId">Id of the team to be updated</param>
        /// <param name="newTeamDto"> DTO of what the team needs to be updated with</param>
        /// <returns></returns>
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

        /// <summary>
        /// Search for one or more teams with a given team name
        /// </summary>
        /// <param name="teamName">String to search for</param>
        /// <returns></returns>
        public IEnumerable<TeamDto> SearchTeamDtOs(string teamName)
        {
            try{
               
                var dbTeams = _teamStorageManager.GetAllTeams()
                    .Where(t => t.Name.Equals(teamName));
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
                throw new NullReferenceException("No teams in database, no reason to search my friend");
            }
        }

        /// <summary>
        /// Retrieve a single team from the repository with a given Id
        /// </summary>
        /// <param name="teamId">Id of the team to be returned</param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve all teams from the repository
        /// </summary>
        /// <returns></returns>
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