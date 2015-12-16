using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TeamUserManagement
{
    public interface ITeamManager
    {
        int CreateTeam(TeamDto teamDto);
        bool RemoveTeam(int teamId);
        bool UpdateTeam(int teamId, TeamDto newTeamDto);
        IEnumerable<TeamDto> SearchTeamDtOs(string teamName);
        TeamDto GetTeamDto(int teamId);
        IEnumerable<TeamDto> GetAllTeamDtOs();
    }
}