﻿using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using System.Linq;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class TeamStorageManager 
    {
        IGenericRepository _teamRepo;
        public TeamStorageManager()
        {
            _teamRepo = new EntityFrameworkGenericRepository<StudyContext>();
        }
        
        public TeamStorageManager(IGenericRepository repo)
        {
            _teamRepo = repo;
        }

        public int SaveTeam(Team TeamToSave)
        {
            return _teamRepo.Create(TeamToSave);
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return _teamRepo.Read<Team>();
        }

        public bool RemoveTeam(int TeamWithIDToDelete)
        {
            return _teamRepo.Delete(_teamRepo.Read<Team>(TeamWithIDToDelete));
        }

        public bool UpdateTeam(Team team)
        {            
            return _teamRepo.Update(team);
        }
           
        public Team GetTeam(int TeamID)
        {
            return _teamRepo.Read<Team>(TeamID);
        }
       

    }
   
}
