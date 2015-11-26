using Storage.Repository;
using System;
using System.Collections.Generic;
using Logic.Model.DTO;
using Logic.Model;

namespace Logic.StorageManagement
{
    public class StudyStorageManager
    {
        IRepository _studyRepo;
        public IEnumerable<StudyLogic> Tasks => _studyRepo.Read<StudyLogic>();
        public StudyStorageManager()
        {
        }

        public StudyStorageManager(IRepository repo)
        {
            _studyRepo = repo;
        }

        public StudyLogic saveStudy(StudyLogic study)
        {

            var newStudy = study;  
            _studyRepo.Create(newStudy);
            return newStudy;
        }

        //public List<Study>

    }
}
