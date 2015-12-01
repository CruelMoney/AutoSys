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

        public List<TaskRequest> GetTasksForUser(int userId, StudyLogic study, int count, TaskRequest.Filter filter, TaskRequest.Type type)
        {
            StageLogic currentStageLogic = null;
            foreach (var stage in study.Stages)
            {
                if (stage.Id.Equals(study.CurrentStage))
                {
                    currentStageLogic = stage;
                    break;
                }
            }
            var users = study.Team.Users;
            UserLogic currentUser = null;
            foreach (var user in users)
            {
                if (user.Id == userId)
                {
                    currentUser = user;
                    break;
                }
            }
            var tasks = currentStageLogic.UserTasks[currentUser];

            List<TaskRequest> TaskRequestList = null;
            foreach (var taskLogic in tasks)
            {
                TaskRequestList.Add(ConvertToTaskRequest(taskLogic));
            }
            return TaskRequestList;

        }

        public TaskRequest ConvertToTaskRequest(TaskLogic tasklogic)
        {
            TaskRequest taskRequest = new TaskRequest()
            {
                Id = tasklogic.Id,
                TaskType = (TaskRequest.Type)Enum.Parse(typeof(TaskRequest.Type), "both"),
                IsDeliverable = tasklogic.IsDeliverable,
                VisibleFields = null,
                RequestedFields = null

            };
            return taskRequest;
      
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
