using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.CriteriaValidation;
using StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.TaskDistributor;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement
{
    public class TaskManager 
    {
        private readonly DistributorSelector _taskDistributor;
        private readonly TaskGenerator _taskGenerator;
        private readonly TaskStorageManager _storageManager;
        private readonly CriteriaValidator _criteriaValidator;
        private IDisposable _unsubscriber;

        public TaskManager()
        {
            _taskDistributor = new DistributorSelector();
            _criteriaValidator = new CriteriaValidation.CriteriaValidator();
            _taskGenerator = new TaskGenerator();
            _storageManager = new TaskStorageManager();
        }

        public TaskManager(TaskStorageManager storageManager)
        {
            _taskDistributor = new DistributorSelector();
            _criteriaValidator = new CriteriaValidation.CriteriaValidator();
            _taskGenerator = new TaskGenerator();
            _storageManager = storageManager;
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
        public IEnumerable<int> GenerateValidationTasks(ICollection<int> taskIDs, ICollection<Criteria> criteria, ICollection<User> users, Stage.Distribution distributionRule)
        {
            var validationTasks = new List<StudyTask>();
            
            foreach (var task in taskIDs.Select(taskID => _storageManager.GetTask(taskID)))
            {
                task.IsEditable = false;
                _storageManager.UpdateTask(task);

                //If the task contains conflicting data we create the validate task and save it
                if (task.ContainsConflictingData())
                {
                    validationTasks.Add(_taskGenerator.GenerateValidateTasks(task));
                }
                //If the task fullfills the criteria we return the itemId to be excluded. 
                else if (!TaskMeetsCriteria(criteria, task))
                {
                    yield return task.Paper.Id;
                }
            }

            //Distribute the tasks and save them
            _taskDistributor.Distribute(distributionRule, users, validationTasks).
                ForEach(t=>_storageManager.CreateTask(t));
            
            //If the criteria is fullfilled we dont exclude it and return nothing

        }

        
        public void GenerateReviewTasks(ICollection<Item> items, ICollection<User> users, Stage.Distribution distributionRule, ICollection<Criteria> criteria)
        {
            var reviewTasks = new List<StudyTask>();

            //Generate the tasks for the currentstage
            foreach (var item in items)
            {
                reviewTasks.Add(_taskGenerator.GenerateReviewTask(item, criteria));
            }
            
            //Distribute the tasks and save them
            _taskDistributor.Distribute(distributionRule, users, reviewTasks).
                ForEach(t => _storageManager.CreateTask(t));
        }


        /// <summary>
        /// Validates the tasks using the criteria
        /// </summary>
        /// <param name="criteria">The criteria to validate against</param>
        /// <param name="taskIDs">The tasks to validate</param>
        /// <returns>null if the criteria is met, the id of the tasks item if the criteria is not met</returns>
        public IEnumerable<int> CriteriaValidateTasks(ICollection<Criteria> criteria, ICollection<int> taskIDs)
        {
            foreach (var taskID in taskIDs)
            {
                //The task can no longer be edited at this point
                var task = _storageManager.GetTask(taskID);
                task.IsEditable = false;
                _storageManager.UpdateTask(task);

                if (!TaskMeetsCriteria(criteria, task))
                {
                    yield return task.Paper.Id;
                }
            }
          
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
                Select(t=> _storageManager.GetTask(t)).
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

        /// <summary>
        /// Get the taskRequestDTO for a user
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="taskId">The taskId</param>
        /// <returns></returns>
        public TaskRequestDTO GetTask(int userId, int taskId, ICollection<Item.FieldType> visibleFields)
        {
            StudyTask task;
            try
            {
                task = _storageManager.GetTask(taskId);
            }
            catch (Exception)
            {
                throw;
            }

            return new TaskRequestDTO(task, userId, visibleFields);
        }

        public bool StageIsFinished(Stage stage)
        {
            return stage.TaskIDs.All(taskID => _storageManager.GetTask(taskID).IsFinished());
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

                if (!_criteriaValidator.CriteriaIsMet(criterion, correspondingField.Data))
                {
                    return false;
                }
            }
            return true;
        }


     
        public void OnNext(Study study)
        {
            

        }

    }
}
