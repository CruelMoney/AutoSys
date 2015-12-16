#region Using

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.StudyExecution.TaskManagement.CriteriaValidation;
using StudyConfigurationServer.Logic.StudyExecution.TaskManagement.TaskDistributor;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement
{
    /// <summary>
    /// A manager class responsible for task logic
    /// </summary>
    public class TaskManager : ITaskManager
    {
        private readonly ICriteriaValidator _criteriaValidator;
        private readonly IDistributorSelector _taskDistributor;
        private readonly ITaskStorageManager _storageManager;
        private readonly TaskGenerator _taskGenerator;
        private readonly TaskRequester _taskRequester;

        public TaskManager()
        {
            _taskDistributor = new DistributorSelector();
            _criteriaValidator = new CriteriaValidator();
            _taskGenerator = new TaskGenerator();
            _storageManager = new TaskStorageManager();
            _taskRequester = new TaskRequester(_storageManager);
        }

        public TaskManager(IGenericRepository repo)
        {
            _taskDistributor = new DistributorSelector();
            _criteriaValidator = new CriteriaValidator();
            _taskGenerator = new TaskGenerator();
            _storageManager = new TaskStorageManager(repo);
            _taskRequester = new TaskRequester(_storageManager);
        }

        /// <summary>
        /// Deliver a task by submitting the data for that task
        /// </summary>
        /// <param name="taskId">Id of the task to deliver</param>
        /// <param name="task">The taskDTO with properties to be delivered</param>
        /// <returns></returns>
        public bool DeliverTask(int taskId, TaskSubmissionDto task)
        {
            var taskToUpdate = _storageManager.GetTask(taskId);

            if (!taskToUpdate.IsEditable)
            {
                throw new ArgumentException("The task is not editable");
            }

            taskToUpdate.SubmitData(task);
            _storageManager.UpdateTask(taskToUpdate);

            return true;
        }

        /// <summary>
        /// Generate validation tasks for every review task
        /// </summary>
        /// <param name="reviewTasks">IEnumrable of studyTasks to generate validation task for</param>
        /// <returns></returns>
        public IEnumerable<StudyTask> GenerateValidationTasks(IEnumerable<StudyTask> reviewTasks)
        {
            foreach (var task in reviewTasks)
            {
                //If the task contains conflicting data we create the validate task and save it
                if (task.ContainsConflictingData())
                {
                    yield return _taskGenerator.GenerateValidateTasks(task);
                }
            }
        }
        
        /// <summary>
        /// Generate review task with given criteria for every item
        /// </summary>
        /// <param name="items">IEnumerable of items to generate review tasks for.</param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public IEnumerable<StudyTask> GenerateReviewTasks(IEnumerable<Item> items, List<Criteria> criteria)
        {
            //Generate the tasks for the currentstage
            foreach (var item in items)
            {
                yield return _taskGenerator.GenerateReviewTask(item, criteria);
            }
        }

        public IEnumerable<StudyTask> Distribute(IEnumerable<User> users, Stage.Distribution distributionRule,
            IEnumerable<StudyTask> tasks)
        {
            //Only distribute tasks that are editable
            return _taskDistributor.Distribute(distributionRule, users,
                tasks.Where(t => t.IsEditable));
        }
        
       public IEnumerable<Item> GetExcludedItems(ICollection<StudyTask> tasks, ICollection<Criteria> criteria)
        {
            foreach (var task in tasks)
            {
                if (!task.ContainsConflictingData() && task.IsFinished())
                {
                    task.IsEditable = false;
                    //If the task does not meet the criteria we return the itemId to be excluded. 
                    if (!TaskMeetsCriteria(criteria, task))
                    {
                        yield return task.Paper;
                    }
                }
            }
        }

         public IEnumerable<TaskRequestDto> GetTasksDtOs(ICollection<FieldType> visibleFields, List<int> taskIDs,
            int userId, int count, TaskRequestDto.Filter filter, TaskRequestDto.Type type)
        {
            var tasks = _taskRequester.GetTasks(taskIDs, userId, count, filter, type);

            return from StudyTask task in tasks
                select new TaskRequestDto(task, visibleFields, userId);
        }

      public IEnumerable<int> GetTasksIDs(List<int> taskIDs, int userId, TaskRequestDto.Filter filter,
            TaskRequestDto.Type type)
        {
            return _taskRequester.GetTaskIDs(taskIDs, userId, filter, type);
        }


        public int CreateTask(StudyTask task)
        {
            return _storageManager.CreateTask(task);
        }


        public ResourceDto GetResource(int id, int resourceId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Checks if a task meets the stages various criteria.
        ///     Each of the tasks fields are checked against the corresponding criteria.
        /// </summary>
        /// <param name="criteria"></param>
        /// <param name="task">The task we are checking</param>
        /// <returns></returns>
        private bool TaskMeetsCriteria(ICollection<Criteria> criteria, StudyTask task)
        {
            if (criteria.Count == 0)
            {
                throw new ArgumentException("the stage does not contain any criteria");
            }

            foreach (var criterion  in criteria)
            {
                //Finds the corresponding field for the criteria using the name...
                var correspondingField = task.DataFields.First(f => f.Name.Equals(criterion.Name)).UserData.First();

                if (!_criteriaValidator.CriteriaIsMet(criterion, correspondingField.Data.Select(s => s.Value).ToArray()))
                {
                    return false;
                }
            }
            return true;
        }

        public bool TaskIsFinished(int taskId)
        {
            return _storageManager.GetTask(taskId).IsFinished();
        }

        public TaskRequestDto GetTaskDto(int taskId, int? userId = null)
        {
            var task = _storageManager.GetAllTasks()
                .Where(t => t.ID == taskId)
                .Include(t => t.Stage)
                .FirstOrDefault();

            if (task == null)
            {
                throw new NullReferenceException("the task does not exist");
            }

            return new TaskRequestDto(task, task.Stage.VisibleFields, userId);
        }
    }
}