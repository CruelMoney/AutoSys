﻿using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class StudyStorageManager
    {
        IGenericRepository _studyRepo;
        public StudyStorageManager()
        {
            _studyRepo = new EntityFrameworkGenericRepository<StudyDataContext>();
        }

        public StudyStorageManager(IGenericRepository repo)
        {
            _studyRepo = repo;
        }

        public int SaveStudy(Study study)
        {
            return _studyRepo.Create(study);
        }

        public bool RemoveStudy(int studyWithIdToDelete)
        {
            return _studyRepo.Delete(_studyRepo.Read<Study>(studyWithIdToDelete));
        }
        public bool UpdateStudy(Study study)
        {
            return _studyRepo.Update(study);
        }

        public IEnumerable<Study> GetAllStudies()
        {
            return _studyRepo.Read<Study>();
        }

        public Study GetStudy(int studyId)
        {
            return _studyRepo.Read<Study>(studyId);
        }

    }
}