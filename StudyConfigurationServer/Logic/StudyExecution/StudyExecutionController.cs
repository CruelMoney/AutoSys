using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.StudyExecution.TaskManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.StudyExecution
{
    /// <summary>
    /// Class responsible for handling the logic within a given study
    /// </summary>
    public class StudyExecutionController : IStudyExecutionController
    {
        private readonly IStudyStorageManager _studyStorageManager;
        private readonly ITaskManager _taskManager;


        public StudyExecutionController()
        {
            var repo = new EntityFrameworkGenericRepository<StudyContext>();
            _studyStorageManager = new StudyStorageManager(repo);
            _taskManager = new TaskManager(repo);
        }

        public StudyExecutionController(IStudyStorageManager storageManager, ITaskManager taskManager)
        {
            _studyStorageManager = storageManager;
            _taskManager = taskManager;
        }

        /// <summary>
        /// Start the study by starting a review stage
        /// </summary>
        /// <param name="study">Study to start</param>
        public void ExecuteStudy(Study study)
        {
            StartReviewPhase(study);
        }

        /// <summary>
        /// Deliver a task
        /// </summary>
        /// <param name="studyId">Id of the study to deliver a task to</param>
        /// <param name="taskId">Id of the task to deliver</param>
        /// <param name="taskDto">TaskDTO containing the properties of the task be delivered</param>
        public void DeliverTask(int studyId, int taskId, TaskSubmissionDto taskDto)
        {
            var currentStudy = _studyStorageManager.GetAll()
                .Where(s => s.ID == studyId)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

            if (currentStudy == null)
            {
                throw new NullReferenceException("Study not found");
            }
            if (!currentStudy.Team.Users.Select(u => u.ID).Contains(taskDto.UserId))
            {
                throw new ArgumentException("The user is not part of this study");
            }

            _taskManager.DeliverTask(taskId, taskDto);

            //Determine if the stage is finished
            if (currentStudy.CurrentStage().
                Tasks.TrueForAll(t => t.IsFinished()))
            {
                MoveToNextPhase(currentStudy);
            }
        }

        /// <summary>
        /// Make a stage transition, based on the current task.Type
        /// </summary>
        /// <param name="study">Study to switch stage in</param>
        private void MoveToNextPhase(Study study)
        {
            var currentStage = study.CurrentStage();

            switch (currentStage.CurrentTaskType)
            {
                case StudyTask.Type.Review:
                    FinishReviewPhase(study);
                    break;
                case StudyTask.Type.Conflict:
                    FinishConflictPhase(study);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _studyStorageManager.Update(study);
        }

        /// <summary>
        /// Conclude a review stage. If no more validation tasks, finish the conflictStage
        /// </summary>
        /// <param name="study"></param>
        private void FinishReviewPhase(Study study)
        {
            var currentStage = study.CurrentStage();

            //Start the validation phase and check if any tasks have been generated
            var validationTasks = StartConflictPhase(study).ToList();

            if (!validationTasks.Any())
            {
                //Finish the phase if no validation tasks
                FinishConflictPhase(study);
            }
        }

        /// <summary>
        /// Finish a conflict stage. By validating the tasks and remove the excluded items from the study.
        /// Start a new review stage if the study is not finished
        /// </summary>
        /// <param name="study"></param>
        private void FinishConflictPhase(Study study)
        {
            var currentStage = study.CurrentStage();
            var tasks = currentStage.Tasks;
            var criteria = currentStage.Criteria;

            //If there's any validation tasks we validate them
            if (tasks.Any())
            {
                var excludedItems = _taskManager.GetExcludedItems(tasks, criteria);

                //Remove the excluded items from the study
                study.Items.RemoveAll(i => excludedItems.Contains(i));
            }

            //move to the next stage
            study.MoveToNextStage();
            if (!study.IsFinished)
            {
                StartReviewPhase(study);
            }
        }

        /// <summary>
        /// Start a review stage by generating review tasks.
        /// The tasks that can be automatically filled out by the TaskManager will be validated by the TaskManager. 
        /// Remove the excluded items from the study. If then no tasks are left, we finish the conflict phase to go directly to the next stage. 
        /// </summary>
        /// <param name="study">study to begin review stage on</param>
        /// <returns></returns>
        private IEnumerable<StudyTask> StartReviewPhase(Study study)
        {
            //Find the current stage
            var stage = study.CurrentStage();
            stage.CurrentTaskType = StudyTask.Type.Review;

            //Find the reviewers
            var reviewers = stage.Users.Where(u => u.StudyRole == UserStudies.Role.Reviewer).Select(u => u.User);

            //Generate the tasks
            var reviewTasks = _taskManager.GenerateReviewTasks(study.Items, stage.Criteria).ToList();

            //Autoexlcuded items that have been filled out  
            var excludedItems = _taskManager.GetExcludedItems(reviewTasks, stage.Criteria).ToList();

            //Remove the excluded items from the study, and move to next stage
            study.Items.RemoveAll(i => excludedItems.Contains(i));

            //Distribute the tasks
            var tasks = _taskManager.Distribute(reviewers, stage.DistributionRule,
                _taskManager.GenerateReviewTasks(study.Items, stage.Criteria)).ToList();

            //Set the tasks in the stage to the reviewTasks
            stage.Tasks = tasks.ToList();

            if (!tasks.Any())
            {
                FinishConflictPhase(study);
            }

            return tasks;
        }

        /// <summary>
        /// Start a conflict stage. Validation tasks based on the existing conflicting tasks.
        /// </summary>
        /// <param name="study">Study to start conflict stage on</param>
        /// <returns></returns>
        private IEnumerable<StudyTask> StartConflictPhase(Study study)
        {
            var stage = study.CurrentStage();

            var tasks = stage.Tasks.Where(t => t.ContainsConflictingData());
            var validators = stage.Users.Where(u => u.StudyRole == UserStudies.Role.Validator).Select(u => u.User);

            //Update stage to start the validations and generate the validation tasks
            stage.CurrentTaskType = StudyTask.Type.Conflict;
            var validationTasks = _taskManager.GenerateValidationTasks(tasks);

            //Always distribute validation tasks with no overlap
            var distributedTasks = _taskManager.Distribute(validators, Stage.Distribution.NoOverlap, validationTasks).ToList();

            stage.Tasks.AddRange(distributedTasks);

            return distributedTasks;
        }
        
        /// <summary>
        /// Get tasks from a given study for a given user. 
        /// </summary>
        /// <param name="studyId">Id of the study to retrievetasks from</param>
        /// <param name="userId">Id of the user to retrieve tasks for</param>
        /// <param name="count">The amount of tasks to return</param>
        /// <param name="filter">Filter what kinds of tasks to list (remaining, editible, done)</param>
        /// <param name="type">Choose whether review or conflict tasks to be returned or both</param>
        /// <returns></returns>
        public IEnumerable<TaskRequestDto> GetTasks(int studyId, int userId, int count, TaskRequestDto.Filter filter,
            TaskRequestDto.Type type)
        {
            var study = _studyStorageManager.GetAll()
                .Where(s => s.ID == studyId)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

            if (study == null)
            {
                throw new NullReferenceException("Study not found");
            }
            if (!study.Team.Users.Select(u => u.ID).Contains(userId))
            {
                throw new ArgumentException("The user is not part of this study");
            }

            var taskIDs = study.CurrentStage().Tasks.Select(t => t.ID).ToList();
            var visibleFields = study.CurrentStage().VisibleFields;

            return _taskManager.GetTasksDtOs(visibleFields, taskIDs, userId, count, filter, type);
        }

        /// <summary>
        /// Return the ids of all tasks for a given user
        /// </summary>
        /// <param name="studyId">Id of the study to retrieve tasks for</param>
        /// <param name="userId">Id of the user to retrieve tasks for</param>
        /// <param name="filter">Filter what kinds of tasks to list (remaining, editible, done)</param>
        /// <param name="type">Choose whether review or conflict tasks to be returned or both</param>
        /// <returns></returns>
        public IEnumerable<int> GetTasksIDs(int studyId, int userId, TaskRequestDto.Filter filter,
            TaskRequestDto.Type type)
        {
            var study = _studyStorageManager.GetAll()
                .Where(s => s.ID == studyId)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

            if (study == null)
            {
                throw new NullReferenceException("Study not found");
            }
            if (!study.Team.Users.Select(u => u.ID).Contains(userId))
            {
                throw new ArgumentException("The user is not part of this study");
            }

            var taskIDs = study.CurrentStage().Tasks.Select(t => t.ID).ToList();

            return _taskManager.GetTasksIDs(taskIDs, userId, filter, type);
        }

        /// <summary>
        /// Retrieve a single task with a given Id
        /// </summary>
        /// <param name="taskId">Id of the task to be returned</param>
        /// <returns></returns>
        public TaskRequestDto GetTask(int taskId)
        {
            return _taskManager.GetTaskDto(taskId);
        }
    }
}