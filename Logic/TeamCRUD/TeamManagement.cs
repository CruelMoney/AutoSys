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

        public void CreateTeam()
        {
            //_teamStorageManager.CreateTeam();
            throw new NotImplementedException();
        }

        public void UpdateTeam()
        {
            //_teamStorageManager.UpdateTeam();
            throw new NotImplementedException();
        }

        public void GetTeam()
        {
            //_teamStorageManager.GetTeam();
            throw new NotImplementedException();
        }

        public void DeleteTeam()
        {
            //_teamStorageManager.RemoveTeam();
            throw new NotImplementedException();
        }

    }
}

