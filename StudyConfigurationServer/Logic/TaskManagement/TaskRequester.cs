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


        public TaskRequester(TaskStorageManager storageManager, UserDTO userDto, Study study,
            TaskGenerator taskGenerator)
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
            _taskGenerator = new TaskGenerator();
        }

        public List<TaskRequestDTO> GetTasksForUser(int userId, Study study, int count, TaskRequestDTO.Filter filter,
            TaskRequestDTO.Type type)
        {
            throw new NotImplementedException();



        }

        public TaskRequestDTO GetTask(int id, int taskid)
        {
            throw new NotImplementedException();
        }

           
        }
    }


