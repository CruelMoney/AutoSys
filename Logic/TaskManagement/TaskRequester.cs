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
        private readonly UserDTO _userDto;
        private readonly Study _study;


        public TaskRequester(TaskStorageManager storageManager, UserDTO userDto, Study study, TaskGenerator taskGenerator)
        {
            _storageManager = storageManager;
            _userDto = userDto;
            _study = study;
            _taskGenerator = taskGenerator;
            _storageManager.Subscribe(this as IObserver<TaskRequester>);
        }

        public TaskRequester()
        {
            _storageManager = new TaskStorageManager();
            _userDto = new UserDTO();
            _study = new Study();
            _taskGenerator = new TaskGenerator(_study);
            _storageManager.Subscribe(this as IObserver<TaskRequester>);
        }

        public List<TaskRequestDTO> GetTasksForUser(int userId, Study study, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
      

            var currentUser = (from User user in study.Team.Users
                where user.Id.Equals(userId)
                select user).FirstOrDefault();

            var tasks = from TaskRequestedData task in currentUser.Tasks
                         select ConvertToTaskRequest(task.StudyTask);

            return tasks.ToList();

        }

        public TaskRequestDTO ConvertToTaskRequest(StudyTask task)
        {
            TaskRequestDTO taskRequestDto = new TaskRequestDTO()
            {
                Id = task.Id,
                TaskType = (TaskRequestDTO.Type)Enum.Parse(typeof(TaskRequestDTO.Type), "both"),
                IsDeliverable = task.IsDeliverable,
                VisibleFieldsDto = null,
                RequestedFieldsDto = null

            };
            return taskRequestDto;
      
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
