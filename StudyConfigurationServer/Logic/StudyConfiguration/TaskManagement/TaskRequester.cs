using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        /// <summary>
        /// </summary>
        /// <param name="filter">Defines whether to get remaining tasks, delivered (but still editable) tasks, or completed tasks.</param>
        /// <param name="type">The type of tasks to retrieve.</param>
        public IEnumerable<int> GetTaskIDs(List<int> taskIDs, int userID, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            switch (type)
            {
                case TaskRequestDTO.Type.Conflict:
                    return (from task in GetTasksFiltered(taskIDs, userID, filter)
                        where task.TaskType == StudyTask.Type.Conflict
                        select task.ID);
                case TaskRequestDTO.Type.Review:
                    return (from task in GetTasksFiltered(taskIDs, userID, filter)
                        where task.TaskType == StudyTask.Type.Review
                        select task.ID);
                case TaskRequestDTO.Type.Both:
                    return (from task in GetTasksFiltered(taskIDs, userID, filter)
                        select task.ID);
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        public IEnumerable<StudyTask> GetTasks(List<int> taskIDs, int userID, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            switch (type)
            {
                case TaskRequestDTO.Type.Conflict:
                    return (from task in GetTasksFiltered(taskIDs, userID, filter)
                            where task.TaskType == StudyTask.Type.Conflict
                            select task).Take(count);
                case TaskRequestDTO.Type.Review:
                    return (from task in GetTasksFiltered(taskIDs, userID, filter)
                            where task.TaskType == StudyTask.Type.Review
                            select task).Take(count);
                case TaskRequestDTO.Type.Both:
                    return (from task in GetTasksFiltered(taskIDs, userID, filter)
                            select task).Take(count);
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        public IEnumerable<StudyTask> GetTasksFiltered(List<int> taskIDs, int userID, TaskRequestDTO.Filter filter)
        {
          
            switch (filter)
            {
                case TaskRequestDTO.Filter.Remaining:
                    return GetRemainingTasks(taskIDs, userID);
                case TaskRequestDTO.Filter.Done:
                    return GetFinishedTasks(taskIDs, userID);
                case TaskRequestDTO.Filter.Editable:
                    return GetEditableTasks(taskIDs, userID);
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        private IEnumerable<StudyTask> GetFinishedTasks(List<int> taskIDs, int userID)
        {
            var tasks = _storageManager.GetAllTasks()
              .Where(t => taskIDs.Contains(t.ID))
              .Include(t => t.Users).AsEnumerable();

            tasks = tasks
                .Where(t => t.Users.Select(u => u.ID).Contains(userID))
                .Where(t => !t.IsEditable);
            return tasks;

        }

        private IEnumerable<StudyTask> GetRemainingTasks(List<int> taskIDs, int userID)
        {
            var tasks = _storageManager.GetAllTasks()
                .Where(t => taskIDs.Contains(t.ID))
                .Include(t => t.Users).AsEnumerable();

            tasks = tasks
                .Where(t=>t.Users.Select(u=>u.ID).Contains(userID))
                .Where(t => !t.IsFinished(userID));
            return tasks;
        }

        private IEnumerable<StudyTask> GetEditableTasks(List<int> taskIDs, int userID)
        {
            var tasks = _storageManager.GetAllTasks()
              .Where(t => taskIDs.Contains(t.ID))
              .Include(t => t.Users).AsEnumerable();

            tasks = tasks
                .Where(t => t.Users.Select(u => u.ID).Contains(userID))
                .Where(t => t.IsEditable);
            return tasks;
        }

    }
}