using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public interface IStudyStorageManager
    {
        int Save(Study study);
        bool Remove(int studyWithIdToDelete);
        bool Update(Study study);
        IQueryable<Study> GetAll();
        Study Get(int studyId);
    }
}