using System;
using System.Collections.Generic;
using Logic.Model;
using Logic.StorageManagement;
using Logic.StudyConfiguration.BiblographyParser.bibTex;
using Logic.Model.DTO;
using Logic.StudyConfiguration.BiblographyParser;

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


        public Study NewStudy(string name, TeamDTO teamDto, string studyData)
        {
            throw new NotImplementedException();
        }



        private List<Item> ParseData(string studyData)
        {
            throw new NotImplementedException();
            return _parser.Parse(studyData);
        }
    }
}
