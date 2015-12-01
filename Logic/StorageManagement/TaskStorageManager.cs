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
        public IEnumerable<StudyTask> Tasks => _taskRepo.Read<StudyTask>().Include("UserDTO");

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

        public IEnumerable<TaskRequestDTO> GetTasksForUser(int userID) //ikke sikkert det skal være her
        {
            throw new NotImplementedException();
        }

        public StudyTask FindTaskLogic(TaskRequestDTO task)
        {
            return _taskRepo.Read<StudyTask>().ToList().First(g => g.Id.Equals(task.Id));
        }
    }
}
