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

        public void CreateTeam()
        {
            //_teamSaver.CreateTeam();
            throw new NotImplementedException();
        }

        public void UpdateTeam()
        {
            //_teamSaver.UpdateTeam();
            throw new NotImplementedException();
        }

        public void GetTeam()
        {
            //_teamRequester.GetTeam();
            throw new NotImplementedException();
        }

        public void DeleteTeam()
        {
            //_teamSaver.RemoveTeam();
            throw new NotImplementedException();
        }

    }
}

