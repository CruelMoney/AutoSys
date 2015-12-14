using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement.Interfaces;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class StudyStorageManager : IStudyStorageManager
    {
        readonly IGenericRepository _studyRepo;
      

        public StudyStorageManager()
        {
            _studyRepo = new EntityFrameworkGenericRepository<StudyContext>();
          
        }

       public StudyStorageManager(IGenericRepository repo)
        {
            _studyRepo = repo;
           
        }


        public int Save(Study study)
        {
            var returnValue = _studyRepo.Create(study);

    
            return returnValue;
        }

        public bool Remove(int studyWithIdToDelete)
        {
            return _studyRepo.Delete(_studyRepo.Read<Study>(studyWithIdToDelete));
        }
        public bool Update(Study study)
        {
            var returnValue = _studyRepo.Update(study);


            return returnValue;
        }

        public IQueryable<Study> GetAll()
        {
            return _studyRepo.Read<Study>();
        }

        public Study Get(int studyId)
        {
            return _studyRepo.Read<Study>(studyId);
        }


    }


}
