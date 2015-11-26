﻿using Storage.Repository;
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

        public StudyLogic saveStudy(StudyLogic study)
        {
            _studyRepo.Create(study);
            return study;
        }

        //public List<Study>

    }
}
