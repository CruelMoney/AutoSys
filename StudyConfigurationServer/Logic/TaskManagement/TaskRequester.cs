using System;
using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TaskManagement
{
    public class TaskRequester 
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
         
        }

        public TaskRequester()
        {
            _storageManager = new TaskStorageManager();
            _userDto = new UserDTO();
            _study = new Study();
            _taskGenerator = new TaskGenerator(_study);
        }

        public List<TaskRequestDTO> GetTasksForUser(int userId, Study study, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            throw new NotImplementedException();
            Stage currentStage = null;
            foreach (var stage in study.Stages)
            {
                if (stage.Id.Equals(study.CurrentStage))
                {
                    currentStage = stage;
                    break;
                }
            }

           
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
        
    }
}
