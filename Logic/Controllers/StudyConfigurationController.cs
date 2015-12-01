using System;
using System.Collections.Generic;
using Logic.Model;
using Logic.StorageManagement;
using Logic.StudyConfiguration.BiblographyParser.bibTex;
using Logic.Model.DTO;
using Logic.StudyConfiguration.BiblographyParser;
using System.Windows;
using System.Net;
using System.Web.Http;
using Logic.TeamCRUD;

namespace Logic.StudyConfiguration
{
    public class StudyConfigurationController
    {
        private StudyStorageManager _studyStorageManager;
        private TeamStorageManager _teamStorageManager;
        private BibTexParser _parser = new BibTexParser(new ItemValidator());


        public StudyConfigurationController()
        {
            _studyStorageManager = new StudyStorageManager();
            _teamStorageManager = new TeamStorageManager();
        }

        public StudyConfigurationController(StudyStorageManager storageManager, TeamStorageManager teamManager)
        {
            _teamStorageManager = teamManager;
            _studyStorageManager = storageManager;
        }


        public Study NewStudy(string name, Team team, string studyDataLocation)
        {
            Study study = new Study() { Name = name, Team = team };
            
            return study;          
        }
     

public IEnumerable<Team> GetTeams(string name = "")
        {
            return _teamStorageManager.SearchTeams(name);
        }



        private List<Item> ParseData(string studyData)
        {
            throw new NotImplementedException();
            return _parser.Parse(studyData);
        }
    }
}
