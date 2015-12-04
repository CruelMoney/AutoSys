using System;
using System.Collections.Generic;
using Storage.Repository;
using StudyConfigurationServer.Logic.TaskManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

namespace StudyConfigurationServer.Logic.StorageManagement
{
    public class TaskStorageManager : IObservable<TaskRequester>
    {
        IGenericRepository _taskRepo;

        public TaskStorageManager()
        {
            _taskRepo = new EntityFrameworkGenericRepository<StudyContext>();
        }

        public TaskStorageManager(IGenericRepository repo)
        {
            _taskRepo = repo;
        }

        public IDisposable Subscribe(IObserver<TaskRequester> observer)
        {
            throw new NotImplementedException();
        }

        public void CreateTask(StudyTask studyTask)
        {
            _taskRepo.Create(studyTask);
        }

        public bool RemoveTask(int taskWithIdToDelete)
        {
            return _taskRepo.Delete(_taskRepo.Read<StudyTask>(taskWithIdToDelete));
        }

        public void UpdateTask(StudyTask studyTask)
        {
            _taskRepo.Update(studyTask);
        }

        public IEnumerable<StudyTask> GetAllTasks() 
        {
            return _taskRepo.Read<StudyTask>();
        }

        public StudyTask GetTask(int taskId)
        {
            return _taskRepo.Read<StudyTask>(taskId);
        }
    }
}
