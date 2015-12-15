#region Using

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement
{
    public class TaskRequester
    {
        private readonly TaskStorageManager _storageManager;
        private Dictionary<TaskRequestDto.Type, Func<StudyTask>> _typeSelector;

        public TaskRequester(TaskStorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        public TaskRequester()
        {
            _storageManager = new TaskStorageManager();
        }

        /// <summary>
        /// </summary>
        /// <param name="filter">Defines whether to get remaining tasks, delivered (but still editable) tasks, or completed tasks.</param>
        /// <param name="type">The type of tasks to retrieve.</param>
        public IEnumerable<int> GetTaskIDs(List<int> taskIDs, int userId, TaskRequestDto.Filter filter,
            TaskRequestDto.Type type)
        {
            switch (type)
            {
                case TaskRequestDto.Type.Conflict:
                    return from task in GetTasksFiltered(taskIDs, userId, filter)
                        where task.TaskType == StudyTask.Type.Conflict
                        select task.ID;
                case TaskRequestDto.Type.Review:
                    return from task in GetTasksFiltered(taskIDs, userId, filter)
                        where task.TaskType == StudyTask.Type.Review
                        select task.ID;
                case TaskRequestDto.Type.Both:
                    return from task in GetTasksFiltered(taskIDs, userId, filter)
                        select task.ID;
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        public IEnumerable<StudyTask> GetTasks(List<int> taskIDs, int userId, int count, TaskRequestDto.Filter filter,
            TaskRequestDto.Type type)
        {
            switch (type)
            {
                case TaskRequestDto.Type.Conflict:
                    return (from task in GetTasksFiltered(taskIDs, userId, filter)
                        where task.TaskType == StudyTask.Type.Conflict
                        select task).Take(count);
                case TaskRequestDto.Type.Review:
                    return (from task in GetTasksFiltered(taskIDs, userId, filter)
                        where task.TaskType == StudyTask.Type.Review
                        select task).Take(count);
                case TaskRequestDto.Type.Both:
                    return (from task in GetTasksFiltered(taskIDs, userId, filter)
                        select task).Take(count);
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        public IEnumerable<StudyTask> GetTasksFiltered(List<int> taskIDs, int userId, TaskRequestDto.Filter filter)
        {
            switch (filter)
            {
                case TaskRequestDto.Filter.Remaining:
                    return GetRemainingTasks(taskIDs, userId);
                case TaskRequestDto.Filter.Done:
                    return GetFinishedTasks(taskIDs, userId);
                case TaskRequestDto.Filter.Editable:
                    return GetEditableTasks(taskIDs, userId);
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        private IEnumerable<StudyTask> GetFinishedTasks(List<int> taskIDs, int userId)
        {
            var tasks = _storageManager.GetAllTasks()
                .Where(t => taskIDs.Contains(t.ID))
                .Include(t => t.Users).AsEnumerable();

            tasks = tasks
                .Where(t => t.Users.Select(u => u.ID).Contains(userId))
                .Where(t => !t.IsEditable);
            return tasks;
        }

        private IEnumerable<StudyTask> GetRemainingTasks(List<int> taskIDs, int userId)
        {
            var tasks = _storageManager.GetAllTasks()
                .Where(t => taskIDs.Contains(t.ID))
                .Include(t => t.Users).AsEnumerable();

            tasks = tasks
                .Where(t => t.Users.Select(u => u.ID).Contains(userId))
                .Where(t => !t.IsFinished(userId));
            return tasks;
        }

        private IEnumerable<StudyTask> GetEditableTasks(List<int> taskIDs, int userId)
        {
            var tasks = _storageManager.GetAllTasks()
                .Where(t => taskIDs.Contains(t.ID))
                .Include(t => t.Users).AsEnumerable();

            tasks = tasks
                .Where(t => t.Users.Select(u => u.ID).Contains(userId))
                .Where(t => t.IsEditable);
            return tasks;
        }
    }
}