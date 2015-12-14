using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Storage.Repository;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StorageManagement.Interfaces
{
    public interface IStudyStorageManager
    {

        int Save(Study t);
        bool Remove(int id);
        bool Update(Study t);
        IQueryable<Study> GetAll();
        Study Get(int id);
    }
}