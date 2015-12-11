using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement
{
    public class TaskRequester
    {
        TaskStorageManager _storageManager;
        Dictionary<TaskRequestDTO.Type, Func<StudyTask>> typeSelector; 

        public TaskRequester(TaskStorageManager storageManager)
        {
            _storageManager = storageManager;
         
        }

        public IEnumerable<TaskRequestDTO> GetTasksForUser(Study study, int userID, int count = 1, TaskRequestDTO.Filter filter = TaskRequestDTO.Filter.Remaining, StudyTask.Type type = StudyTask.Type.Both)
        {
            var visibleFields = study.CurrentStage().VisibleFields;

            switch (filter)
            {
                case TaskRequestDTO.Filter.Remaining:
                    return GetRemainingTasks(study, userID).
                        Where(t => t.TaskType == type).
                        Take(count).
                        Select(task => new TaskRequestDTO(task, userID, visibleFields));
                case TaskRequestDTO.Filter.Done:
                    return GetFinishedTasks(study, userID).
                        Where(t => t.TaskType == type).
                        Take(count).
                        Select(task => new TaskRequestDTO(task, userID, visibleFields));
                case TaskRequestDTO.Filter.Editable:
                    return GetEditableTasks(study, userID).
                        Where(t => t.TaskType == type).
                        Take(count).
                        Select(task => new TaskRequestDTO(task, userID, visibleFields));
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        /// <summary>
        /// Get requested StudyTask IDs for a specific User of a given study. By default, delivered but still editable StudyTask IDs are returned.
        /// Optionally, the type of StudyTask IDs to retrieve are specified.
        /// </summary>
        /// <param name="user">The User to get tasks for</param>
        /// <param name="filter">Defines whether to get remaining tasks, delivered (but still editable) tasks, or completed tasks.</param>
        /// <param name="type">The type of tasks to retrieve.</param>
        /// <param name="study">The study to get tasks for.</param>
        public IEnumerable<int> GetTaskIDs(Study study, int userID, TaskRequestDTO.Filter filter = TaskRequestDTO.Filter.Editable, StudyTask.Type type = StudyTask.Type.Both)
        {
            switch (filter)
            {
                case TaskRequestDTO.Filter.Remaining:
                    return GetRemainingTasks(study, userID).Where(t => t.TaskType == type).Select(t => t.Id);
                case TaskRequestDTO.Filter.Done:
                    return GetFinishedTasks(study, userID).Where(t => t.TaskType == type).Select(t => t.Id);
                case TaskRequestDTO.Filter.Editable:
                    return GetEditableTasks(study, userID).Where(t => t.TaskType == type).Select(t => t.Id);
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        private IEnumerable<StudyTask> GetFinishedTasks(Study study, int userID)
        {
            return study.Stages.
                SelectMany(stage => stage.TaskIDs).
                Select(t => _storageManager.GetTask(t)).
                Where(t => t.UserIDs.Contains(userID)).
                Where(t => !t.IsEditable);
        }

        private IEnumerable<StudyTask> GetRemainingTasks(Study study, int userID)
        {
            return study.Stages.
              SelectMany(stage => stage.TaskIDs).
              Select(t => _storageManager.GetTask(t)).
              Where(t => t.UserIDs.Contains(userID)).
              Where(t => !t.IsFinished(userID));
        }

        private IEnumerable<StudyTask> GetEditableTasks(Study study, int userID)
        {
            return study.Stages.
              SelectMany(stage => stage.TaskIDs).
              Select(t => _storageManager.GetTask(t)).
              Where(t => t.UserIDs.Contains(userID)).
              Where(t => t.IsEditable);
        }

    }
}