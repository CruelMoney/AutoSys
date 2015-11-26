using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.StorageManagement;
using Logic.Model.DTO;
using Logic.Model;

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

        public void CreateTeam(String TeamName, int[] UserIDList, String MetaData)
        {
            var TeamToAdd = new Team();
            TeamToAdd.Name = TeamName;
            TeamToAdd.UserIDs = UserIDList;
            TeamToAdd.Metadata = MetaData;

            _teamStorageManager.SaveTeam(TeamToAdd);
        }

        public void RemoveTeam(int TeamID)
        {
            _teamStorageManager.RemoveTeam(TeamID);
        }

        public void UpdateTeam(int TeamID, String UpdatedName, String UpdatedMetaData)
        {
            var TeamToUpdate = _teamStorageManager.GetTeam(TeamID);         //gets the team to save unupdated fields.
            TeamToUpdate.Name = UpdatedName;
            TeamToUpdate.Metadata = UpdatedMetaData;
            _teamStorageManager.UpdateTeam(TeamToUpdate);
        }

        public IEnumerable<TeamLogic> SearchTeams(String TeamName)
        {
            return _teamStorageManager.SearchTeams(TeamName);
        }

        public TeamLogic GetTeam(int TeamID)
        {
            return _teamStorageManager.GetTeam(TeamID);
        }

    }
}

