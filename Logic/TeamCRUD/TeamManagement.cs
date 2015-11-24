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
        }

        public void RemoveTeam(int TeamID)
        {
            throw new NotImplementedException();
        }

        public void UpdateTeam(int TeamID, String UpdatedName, String UpdatedMetaData)
        {
            throw new NotImplementedException();
        }

        public Boolean TeamExistsByName(String TeamName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Team> SearchTeams(String TeamName)
        {
            throw new NotImplementedException();
        }

        public Team GetTeam(int TeamID)
        {
            throw new NotImplementedException();
        }

    }
}

