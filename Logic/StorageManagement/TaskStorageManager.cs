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

namespace Logic.StorageManagement
{
    public class TaskStorageManager : IObservable<TaskRequester>
    {
        IGenericRepository _taskRepo;
        public IEnumerable<TaskLogic> Tasks => _taskRepo.Read<TaskLogic>().Include("User");

        public TaskStorageManager()
        {
        }

        public TaskStorageManager(IGenericRepository repo)
        {
            _taskRepo = repo;
        }

        public IDisposable Subscribe(IObserver<TaskRequester> observer)
        {
            throw new NotImplementedException();
        }

        public void CreateTask(TaskLogic task)
        {
            _taskRepo.Create(task);
        }

        public void UpdateTask(TaskLogic task)
        {
            _taskRepo.Update(task);
        }

        public IEnumerable<TaskRequest> GetTasksForUser(int userID) //ikke sikkert det skal være her
        {
            throw new NotImplementedException();
        }

        public TaskLogic FindTaskLogic(TaskRequest task)
        {
            return _taskRepo.Read<TaskLogic>().ToList().First(g => g.Id.Equals(task.Id));
        }
    }
}
