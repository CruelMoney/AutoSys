#region Using

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement
{
    /// <summary>
    /// Strategy for getting tasks from the database based on filters and user.
    /// </summary>
    public class TaskRequester
    {
        private readonly ITaskStorageManager _storageManager;
        private Dictionary<TaskRequestDto.Type, Func<StudyTask>> _typeSelector;

        public TaskRequester(ITaskStorageManager storageManager)
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

        /// <summary>
        /// Retrieve all tasks based on their task.type (conflict, review or both)
        /// </summary>
        /// <param name="taskIDs">list of ids of tasks to retrieve</param>
        /// <param name="userId">Id of user who has the tasks to retrieve</param>
        /// <param name="count">number of tasks to return</param>
        /// <param name="filter">the given filter </param>
        /// <param name="type">the given type</param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve tasks based on a filter
        /// </summary>
        /// <param name="taskIDs">List of Ids of tasks to retrieve</param>
        /// <param name="userId">Id of user who has the tasks to retrieve</param>
        /// <param name="filter">a given filter for which tasks to retrieve</param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve all finished tasks
        /// </summary>
        /// <param name="taskIDs">list of taskIds to retrieve</param>
        /// <param name="userId">Id of user who has the given tasks</param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve all remaining tasks
        /// </summary>
        /// <param name="taskIDs">list of taskIds of tasks to retrieve</param>
        /// <param name="userId">Id of user who has the given tasks</param>
        /// <returns></returns>
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

        /// <summary>
        /// Retrieve all editable studytasks
        /// </summary>
        /// <param name="taskIDs">list of taskIds to retrieve</param>
        /// <param name="userId">Id of a user who has these tasks</param>
        /// <returns></returns>
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