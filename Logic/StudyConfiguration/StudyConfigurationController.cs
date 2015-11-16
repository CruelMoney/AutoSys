using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliographyParser;
using Logic.Model;
using Logic.StorageManagement;
using Logic.StudyConfiguration.BiblographyParser.bibTex;

namespace Logic.StudyConfiguration
{
    public class StudyConfigurationController
    {
        private StudyStorageManager _storageManager;
        private TeamStorageManager _teamManager;
        private BibTexParser _parser = new BibTexParser(new ItemValidator());


        public StudyConfigurationController()
        {
            _storageManager = new StudyStorageManager();
            _teamManager = new TeamStorageManager();
        }

        public StudyConfigurationController(StudyStorageManager storageManager, TeamStorageManager teamManager)
        {
            _teamManager = teamManager;
            _storageManager = storageManager;
        }


        public Study NewStudy(string name, Team team, string studyData)
        {
            var data = ParseData(studyData);
           
            var newStudy = _storageManager.saveStudy(name, team, data);
            return newStudy;
        }



        private List<Item> ParseData(string studyData)
        {
            throw new NotImplementedException();
            return _parser.Parse(studyData);
        }
    }
}
