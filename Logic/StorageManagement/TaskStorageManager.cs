using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model;
using Logic.Model.StoredData;
using Logic.TaskManagement;

namespace Logic.StorageManagement
{
    public class TaskStorageManager : IObservable<TaskRequester>
    {

       
        IRepository _taskRepo;
        public IEnumerable<StoredTask> Tasks => _taskRepo.Read<StoredTask>().Include("User");

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

        public void SaveTask(StoredTask task)
        {
            _taskRepo.Create(task);
        }

        public StoredTask FindStoredTask(UserTask task)
        {
            return _taskRepo.Read<StoredTask>().ToList().First(g => g.Id.Equals(task.Id));
        }
    }
}
