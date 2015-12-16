using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.StudyExecution
{
    public interface IStudyExecutionController
    {
        void ExecuteStudy(Study study);

        void DeliverTask(int studyId, int taskId, TaskSubmissionDto taskDto);
        
        IEnumerable<TaskRequestDto> GetTasks(int studyId, int userId, int count, TaskRequestDto.Filter filter,
            TaskRequestDto.Type type);

        IEnumerable<int> GetTasksIDs(int studyId, int userId, TaskRequestDto.Filter filter,
            TaskRequestDto.Type type);

        TaskRequestDto GetTask(int taskId);
    }
}