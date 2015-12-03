using StudyConfigurationServer.Logic.StorageManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.StudyConfiguration
{
    public class StudyManager
    {
        private readonly StudyStorageManager _studyStorageManager;
        public StudyManager()
        {
            _studyStorageManager = new StudyStorageManager();
        }

        public StudyManager(StudyStorageManager storageManager)
        {
            _studyStorageManager = storageManager;
        }
        public int CreateStudy(Study study)
        {
            var studyToAdd = new Study
            {
                Name = study.Name,
                Validators = study.Validators,
                Reviewers = study.Reviewers,
                IsFinished = false
            };

            return _studyStorageManager.SaveStudy(studyToAdd);
        }

        public bool RemoveStudy(int studyId)
        {
            return _studyStorageManager.RemoveStudy(studyId);
        }

        public bool UpdateStudy(int studyId, Study study)
        {

            return _studyStorageManager.UpdateStudy(study);
        }

        public IEnumerable<Study> SearchStudies(string studyName)
        {
            return
                (from Study dbStudy in _studyStorageManager.GetAllStudies()
                 where dbStudy.Name.Equals(studyName)
                 select dbStudy)
                    .ToList();
        }

        public Study GetStudy(int studyId)
        {
            return _studyStorageManager.GetStudy(studyId);
            
        }

        public IEnumerable<Study> GetAllStudies()
        {
            return
                (from Study dbStudy in _studyStorageManager.GetAllStudies()
                 select dbStudy).ToList();
        }

        
    }
   

}