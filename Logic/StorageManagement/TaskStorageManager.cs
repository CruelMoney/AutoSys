using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model;
using Logic.TaskManagement;
using Logic.Model.Data;
using Logic.Model.DTO;

namespace Logic.StorageManagement
{
    public class TaskStorageManager : IObservable<TaskRequester>
    {
        IRepository _taskRepo;
        public IEnumerable<StoredTaskRequest> Tasks => _taskRepo.Read<StoredTaskRequest>().Include("User");

        public TaskStorageManager()
        {
        }

        public TaskStorageManager(IRepository repo)
        {
            _taskRepo = repo;
        }

        public IDisposable Subscribe(IObserver<TaskRequester> observer)
        {
            throw new NotImplementedException();
        }

        public void CreateTask(StoredTaskRequest task)
        {
            _taskRepo.Create(task);
        }

        public void UpdateTask(StoredTaskRequest task)
        {
            _taskRepo.Update(task);
        }

        public IEnumerable<TaskRequest> GetTasksForUser(int userID) //ikke sikkert det skal være her
        {
            throw new NotImplementedException();
        }

        public StoredTaskRequest FindStoredTask(TaskRequest task)
        {
            return _taskRepo.Read<StoredTaskRequest>().ToList().First(g => g.Id.Equals(task.Id));
        }
    }
}
