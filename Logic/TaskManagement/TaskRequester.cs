using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Data;
using Logic.StorageManagement;

namespace Logic.TaskManagement
{
    public class TaskRequester : IObserver<TaskStorageManager>
    {
        private TaskGenerator _taskGenerator;
        private TaskStorageManager _storageManager;
        private readonly User _user;
        private readonly Study _study;


        public TaskRequester(TaskStorageManager storageManager, User user, Study study, TaskGenerator taskGenerator)
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
            _study = new Study();
            _taskGenerator = new TaskGenerator(_study);
            _storageManager.Subscribe(this as IObserver<TaskRequester>);
        }

        public void GetTaskForUser(User user, Study study)
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
