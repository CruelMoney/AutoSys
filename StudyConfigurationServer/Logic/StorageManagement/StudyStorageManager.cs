using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class StudyStorageManager 
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


        public int SaveStudy(Study study)
        {
            var returnValue = _studyRepo.Create(study);

    
            return returnValue;
        }

        public bool RemoveStudy(int studyWithIdToDelete)
        {
            return _studyRepo.Delete(_studyRepo.Read<Study>(studyWithIdToDelete));
        }
        public bool UpdateStudy(Study study)
        {
            var returnValue = _studyRepo.Update(study);


            return returnValue;
        }

        public IQueryable<Study> GetAllStudies()
        {
            return _studyRepo.Read<Study>();
        }

        public Study GetStudy(int studyId)
        {
            return _studyRepo.Read<Study>(studyId);
        }


      


    }


}
