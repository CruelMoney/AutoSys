using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.StorageManagement;

namespace Logic.TeamCRUD
{
    public class TeamManagement
    {
        TeamStorageManager _teamStorageManager;

        public TeamManagement()
        {
            _teamStorageManager = new TeamStorageManager();
        }

        public TeamManagement(TeamStorageManager storageManager)
        {
            _teamStorageManager = storageManager;
        }

        public void CreateTeam(String TeamName, IEnumerable<User> UserList, String MetaData)
        {
            throw new NotImplementedException();
            //_teamStorageManager.SaveTeam(new Model.DTO.Team()) Opret team.
        }

        public void RemoveTeam(int TeamID)
        {
            throw new NotImplementedException();
            _teamStorageManager.RemoveTeam(TeamID);
        }

        public void UpdateTeam(int TeamID, String UpdatedName, String UpdatedMetaData)
        {
            throw new NotImplementedException();
            //_teamStorageManager.UpdateTeam(new Team()) opret team med samme id og nyt data
        }

        public IEnumerable<Team> SearchTeams(String TeamName)
        {
            throw new NotImplementedException();
            _teamStorageManager.SearchTeams(TeamName);
        }

        public Team GetTeam(int TeamID)
        {
            throw new NotImplementedException();
            _teamStorageManager.GetTeam(TeamID);
        }

    }
}

