using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.Ajax.Utilities;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.CriteriaValidation;
using StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.TaskDistributor;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;
using FieldType = StudyConfigurationServer.Models.FieldType;

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement
{
    public class TaskManager 
    {
        private readonly DistributorSelector _taskDistributor;
        private readonly TaskGenerator _taskGenerator;
        private readonly TaskRequester _taskRequester;
        private readonly TaskStorageManager _storageManager;
        private readonly CriteriaValidator _criteriaValidator;
        private IDisposable _unsubscriber;

        public TaskManager()
        {
            _taskDistributor = new DistributorSelector();
            _criteriaValidator = new CriteriaValidator();
            _taskGenerator = new TaskGenerator();
            _storageManager = new TaskStorageManager();
            _taskRequester = new TaskRequester(_storageManager);
        }

        public TaskManager(EntityFrameworkGenericRepository<StudyContext> repo)
        {
            _taskDistributor = new DistributorSelector();
            _criteriaValidator = new CriteriaValidator();
            _taskGenerator = new TaskGenerator();
            _storageManager = new TaskStorageManager(repo);
            _taskRequester = new TaskRequester(_storageManager);
        }

        public bool DeliverTask(int taskID, TaskSubmissionDTO task)
        {
            var taskToUpdate = _storageManager.GetTask(taskID);

            if (!taskToUpdate.IsEditable)
            {
                return false;
                throw new InvalidOperationException("The task is not editable");
            }
            
            taskToUpdate.SubmitData(task);
            _storageManager.UpdateTask(taskToUpdate);

            return true;
        }

        /// <summary>
        /// Generates a validating task and saves it.
        /// If the task does not contain conflicting and it doesn't fulfill the criteria data we return it's item.
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns>itemIDs to be excluded from study</returns>
        /// TODO what happens if there are no validators for this stage
        public IEnumerable<StudyTask> GenerateValidationTasks(ICollection<int> taskIDs, ICollection<Criteria> criteria, ICollection<User> users, Stage.Distribution distributionRule)
        {
            var validationTasks = new List<StudyTask>();
            
            foreach (var task in taskIDs.Select(taskID => _storageManager.GetTask(taskID)))
            {
                task.IsEditable = false;
                _storageManager.UpdateTask(task);

                //If the task contains conflicting data we create the validate task and save it
                if (task.ContainsConflictingData())
                {
                   yield return _taskGenerator.GenerateValidateTasks(task);
                }
            }

            //Distribute the tasks and save them
            _taskDistributor.Distribute(distributionRule, users, validationTasks);

        }

      
        public IEnumerable<StudyTask> GenerateReviewTasks(ICollection<Item> items, ICollection<User> users, List<Criteria> criteria, Stage.Distribution distribution)
        {
            var reviewTasks = new List<StudyTask>();

            //Generate the tasks for the currentstage
            foreach (var item in items)
            {
                reviewTasks.Add(_taskGenerator.GenerateReviewTask(item, criteria));
            }
            
            //Distribute the tasks and save them
            var tasks = _taskDistributor.Distribute(distribution, users, reviewTasks).ToList();
              
            return tasks;
        }


        /// <summary>
        /// Generates a validating task and saves it.
        /// If the task does not contain conflicting and it doesn't fulfill the criteria data we return it's item.
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns>itemIDs to be excluded from study</returns>
        /// TODO what happens if there are no validators for this stage
        public IEnumerable<int> GetExcludedItems(ICollection<int> taskIDs, ICollection<Criteria> criteria)
        {
            foreach (var task in taskIDs.Select(taskID => _storageManager.GetTask(taskID)))
            {
                //If the task contains conflicting data we create the validate task and save it
                if (!task.ContainsConflictingData())
                {
                    //If the task does not meet the criteria we return the itemId to be excluded. 
                    if (!TaskMeetsCriteria(criteria, task))
                    {
                        yield return task.Paper.ID;
                    }
                }
            }
        }

        /// <summary>
        /// Get the taskRequestDTO for a user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TaskRequestDTO> GetTasksDTOs(ICollection<FieldType> visibleFields, List<int> taskIDs, int userID, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            var tasks = _taskRequester.GetTasks(taskIDs, userID, count, filter, type);

            return from StudyTask task in tasks
                select new TaskRequestDTO(task, userID, visibleFields);
        }

        /// <summary>
        /// Get all the taskIDs for a user
        /// </summary>
        /// <returns></returns>
        public IEnumerable<int> GetTasksIDs(List<int> taskIDs, int userID, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            return _taskRequester.GetTaskIDs(taskIDs, userID, filter, type);
        }


        public int CreateTask(StudyTask task)
        {
            return _storageManager.CreateTask(task);
        }


        
        /// <summary>
        /// Returns the resource with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the study this resource is part of.</param>
        /// <param name="resourceId">The ID of the requested resource.</param>
        public ResourceDTO GetResource(int id, int resourceId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if a task meets the stages various criteria.
        /// Each of the tasks fields are checked against the corresponding criteria.
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

                if (!_criteriaValidator.CriteriaIsMet(criterion, correspondingField.Data.Select(s=>s.Value).ToArray()))
                {
                    return false;
                }
            }
            return true;
        }

        public bool TaskIsFinished(int taskID)
        {
            return _storageManager.GetTask(taskID).IsFinished();
        }
     
        public TaskRequestDTO GetTaskDTO(int userID, int taskId)
        {
            var task = _storageManager.GetAllTasks()
                .Where(t=>t.ID==taskId)
                .Include(t=>t.Stage)
                .FirstOrDefault();

            return new TaskRequestDTO(task, userID, task.Stage.VisibleFields);
        }
    }
}
