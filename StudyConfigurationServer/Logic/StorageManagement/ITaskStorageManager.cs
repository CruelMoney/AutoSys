#region Using

using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

#endregion

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public interface ITaskStorageManager
    {

        int CreateTask(StudyTask studyTask);

        bool RemoveTask(int taskWithIdToDelete);

        bool UpdateTask(StudyTask studyTask);

        IQueryable<StudyTask> GetAllTasks();

        StudyTask GetTask(int taskId);
    }
}