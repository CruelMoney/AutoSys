using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model;
using Logic.StorageManagement;
using Logic.Model.DTO;

namespace Logic.TaskManagement
{
    public class TaskRequester : IObserver<TaskStorageManager>
    {
        private TaskGenerator _taskGenerator;
        private TaskStorageManager _storageManager;
        private readonly User _user;
        private readonly StudyLogic _study;


        public TaskRequester(TaskStorageManager storageManager, User user, StudyLogic study, TaskGenerator taskGenerator)
        {
            _storageManager = storageManager;
            _user = user;
            _study = study;
            _taskGenerator = taskGenerator;
            _storageManager.Subscribe(this as IObserver<TaskRequester>);
        }

        public TaskRequester()
        {
            _storageManager = new TaskStorageManager();
            _user = new User();
            _study = new StudyLogic();
            _taskGenerator = new TaskGenerator(_study);
            _storageManager.Subscribe(this as IObserver<TaskRequester>);
        }

        public void GetTaskForUser(User user, StudyLogic study)
        {

            throw new NotImplementedException();
            if (!_storageManager.Tasks.Any())
            {
                ///No tasks
            }
            else
            {
                
            }
        }


        public void OnNext(TaskStorageManager value)
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}
