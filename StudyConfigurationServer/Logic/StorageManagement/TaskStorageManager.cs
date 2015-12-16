#region Using

using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

#endregion

namespace StudyConfigurationServer.Logic.StorageManagement
{
    /// <summary>
    /// StorageManager to create, retrieve, update and delete tasks
    /// </summary>
    public class TaskStorageManager : ITaskStorageManager
    {
        private readonly IGenericRepository _taskRepo;

        public TaskStorageManager()
        {
            _taskRepo = new EntityFrameworkGenericRepository<StudyContext>();
        }

        public TaskStorageManager(IGenericRepository repo)
        {
            _taskRepo = repo;
        }

        /// <summary>
        /// Create a task
        /// </summary>
        /// <param name="studyTask"></param>
        /// <returns></returns>
        public int CreateTask(StudyTask studyTask)
        {
            return _taskRepo.Create(studyTask);
        }

        /// <summary>
        /// Delete a task
        /// </summary>
        /// <param name="taskWithIdToDelete"></param>
        /// <returns></returns>
        public bool RemoveTask(int taskWithIdToDelete)
        {
            return _taskRepo.Delete(_taskRepo.Read<StudyTask>(taskWithIdToDelete));
        }

        /// <summary>
        /// Update a task
        /// </summary>
        /// <param name="studyTask"></param>
        /// <returns></returns>
        public bool UpdateTask(StudyTask studyTask)
        {
            return _taskRepo.Update(studyTask);
        }

        /// <summary>
        /// Retrieve all tasks
        /// </summary>
        /// <returns></returns>
        public IQueryable<StudyTask> GetAllTasks()
        {
            return _taskRepo.Read<StudyTask>();
        }

        /// <summary>
        /// Retrieve a single task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public StudyTask GetTask(int taskId)
        {
            return _taskRepo.Read<StudyTask>(taskId);
        }
    }
}