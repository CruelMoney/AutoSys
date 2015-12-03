using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model;
using Logic.TaskManagement;
using Logic.Model.DTO;
using Logic.Model.Data;

namespace Logic.StorageManagement
{
    public class TaskStorageManager : IObservable<TaskRequester>
    {
        IGenericRepository _taskRepo;

        public TaskStorageManager()
        {
            _taskRepo = new EntityFrameworkGenericRepository<StudyDataContext>();
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
