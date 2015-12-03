using System;
using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TaskManagement
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
            Stage currentStage = null;
            foreach (var stage in study.Stages)
            {
                if (stage.Id.Equals(study.CurrentStage))
                {
                    currentStage = stage;
                    break;
                }
            }

            var currentUser = (from User user in study.Team.Users
                where user.Id.Equals(userId)
                select user).FirstOrDefault();

            var tasks = from StudyTask task in currentUser.Tasks
                         select ConvertToTaskRequest(task);

            return tasks.ToList();
        }

        public TaskRequestDTO ConvertToTaskRequest(StudyTask tasklogic)
        {
            TaskRequestDTO taskRequestDto = new TaskRequestDTO()
            {
                Id = tasklogic.Id,
                TaskType = (TaskRequestDTO.Type)Enum.Parse(typeof(TaskRequestDTO.Type), "both"),
                IsDeliverable = tasklogic.IsDeliverable,
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
