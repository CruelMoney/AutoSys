#region Using

using System.Linq;
using StudyConfigurationServer.Models;

#endregion

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