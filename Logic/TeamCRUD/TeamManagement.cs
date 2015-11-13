using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.StorageManagement;
using Logic.Data;

namespace Logic.TeamCRUD
{
    public class TeamManagement
    {
        TeamSaver _teamSaver;
        TeamRequester _teamRequester;

        public TeamManagement()
        {
            _teamRequester = new TeamRequester();
            _teamSaver = new TeamSaver();
        }

        public TeamManagement(TeamSaver teamSaver)
        {
            _teamSaver = teamSaver;
        }

        public void CreateTeam()
        {
            throw new NotImplementedException();
        }

        public void AddUserToTeam()
        {
            throw new NotImplementedException();
        }

        public void RemoveUserFromTeam()
        {
            throw new NotImplementedException();
        }

        public void UpdateTeam()
        {
            throw new NotImplementedException();
        }

        public void RetrieveTeam()
        {
            throw new NotImplementedException();
        }

        public void DeleteTeam()
        {
            throw new NotImplementedException();
        }

    }
}

