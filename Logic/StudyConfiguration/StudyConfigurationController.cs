using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliographyParser;
using Logic.Model;
using Logic.StorageManagement;
using Logic.StudyConfiguration.BiblographyParser.bibTex;
using Logic.Model.DTO;
using Logic.Model.Data;

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


        public StoredStudy NewStudy(string name, Team team, string studyData)
        {
            var data = ParseData(studyData);
           
            var newStudy = _studyStorageManager.saveStudy(name, team, data);
            return newStudy;
        }



        private List<Item> ParseData(string studyData)
        {
            throw new NotImplementedException();
            return _parser.Parse(studyData);
        }
    }
}
