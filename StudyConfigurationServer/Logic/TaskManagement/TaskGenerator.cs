using System;
using System.Collections.Generic;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TaskManagement
{
    public class TaskGenerator : IObserver<TaskStorageManager>
    {
        public TaskGenerator(Study study)
        {
         
        }

        public IEnumerable<TaskRequestDTO> GenerateTasks(Study study, Stage stage)
        {
            throw new NotImplementedException();
           
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
