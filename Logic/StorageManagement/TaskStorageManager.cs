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

        public void SaveTask(StoredTaskRequest task)
        {
            _taskRepo.Create(task);
        }

        public StoredTaskRequest FindStoredTask(UserTask task)
        {
            return _taskRepo.Read<StoredTask>().ToList().First(g => g.Id.Equals(task.Id));
        }
    }
}
