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

        public void ExecuteStudy(Study study)
        {
            StartReviewPhase(study);
        }

        //TODO check if whole study finished
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


        public TaskRequestDto GetTask(int taskId)
        {
            return _taskManager.GetTaskDto(taskId);
        }
    }
}