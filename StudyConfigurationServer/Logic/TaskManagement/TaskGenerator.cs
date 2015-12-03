using System;
using System.Collections.Generic;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TaskManagement
{
    public class TaskGenerator
    {
        public TaskGenerator(Study study)
        {
         
        }

        public IEnumerable<TaskRequestDTO> GenerateTasks(Study study, Stage stage)
        {
            throw new NotImplementedException();
           
        }

    }
}
