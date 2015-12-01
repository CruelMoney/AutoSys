using Storage.Repository;
using System;
using System.Collections.Generic;
using Logic.Model.DTO;
using Logic.Model;

namespace Logic.StorageManagement
{
    public class StudyStorageManager
    {
        IGenericRepository _studyRepo;
        public StudyStorageManager()
        {
        }

        public StudyStorageManager(IGenericRepository repo)
        {
            _studyRepo = repo;
        }

        public Study saveStudy(Study study)
        {
            _studyRepo.Create(study);
            return study;
        }

        public void removeStudy(Study study)
        {
            _studyRepo.Delete(study);
        }
        public Study GetStudy(int studyid)
        {
            return _studyRepo.Read<Study>(studyid);
        }

        //public List<Study>

    }
}
