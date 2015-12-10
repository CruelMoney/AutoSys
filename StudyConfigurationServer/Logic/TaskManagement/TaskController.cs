using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.TaskManagement.TaskDistributor;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.CriteriaValidator;
using StudyConfigurationServer.Models.DTO;
using static StudyConfigurationServer.Models.DTO.TaskRequestDTO;

namespace StudyConfigurationServer.Logic.TaskManagement
{
    public class TaskController : IObserver<Study>
    {
        private DistributorSelector _taskDistributor;
        private TaskGenerator _taskGenerator;
        private TaskRequester _taskRequester;
        private TaskStorageManager _storageManager;
        private CriteriaValidator _criteriaValidator;

        private IDisposable _unsubscriber;

        public TaskController(TaskStorageManager taskStorage)
        {
            _taskGenerator = new TaskGenerator();
            _storageManager = taskStorage;
            _taskRequester = new TaskRequester();
        }

        public TaskController()
        {
            _taskGenerator = new TaskGenerator();
            _storageManager = new TaskStorageManager();
            _taskRequester = new TaskRequester();

            
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

            //Determine if the stage is finished
            /*
            var currentStage = taskToUpdate.Stage;
            
            if (currentStage.IsFinished())
            {
               MoveToNextStage(currentStage);
            }
            */
        }

   

        private void MoveToNextStage(Stage currentStage)
        {
            if (currentStage.CurrentTaskType == StudyTask.Type.Review)
            {
                //Generate Validationtask for next phase
                var validationTasks = new List<StudyTask>();
                foreach (var task in currentStage.Tasks)
                {
                   
                    if (task.ContainsConflictingData())
                    {
                        validationTasks.Add(_taskGenerator.GenerateValidateTasks(task));
                    }
                    else
                    {
                        CriteriaValidateTask(task.Stage, task);
                    }
                }
                
                //Get validators for this stage
                var validators = currentStage.Users.Where(u => u.StudyRole == UserStudies.Role.Validator).Select(u => u.User).ToList();

                //Distribute the tasks to the validators
                var distributedTasks = _taskDistributor.Distribute(currentStage, validators, validationTasks);

                //Save the tasks in the db
                distributedTasks.ForEach(t => _storageManager.CreateTask(t));

                //Change the stage type to conflict
                currentStage.CurrentTaskType = StudyTask.Type.Conflict;
            }

            //If the stage is at a conflict phase we simply criteriaValidate all the tasks since they cant contain conflicting data.
            else if (currentStage.CurrentTaskType == StudyTask.Type.Conflict)
            {
                foreach (var studyTask in currentStage.Tasks)
                {
                    CriteriaValidateTask(currentStage, studyTask);
                }
                currentStage.Study.MoveToNextStage();
            }
        }

        /// <summary>
        /// CriteriaValidates a task and exclude or include it from the study
        /// </summary>
        /// <param name="stage"></param>
        /// <param name="task"></param>
        private void CriteriaValidateTask(Stage stage, StudyTask task)
        {
            task.IsEditable = false;

            //Include or exclude based on the stage criteria
            if (!TaskMeetsStageCriteria(stage.Criteria, task))
            {
                stage.Study.Items.Remove(task.Paper);
            }

            _storageManager.UpdateTask(task);
        }
       

        public IEnumerable<TaskRequestDTO> GetTasksForUser(Study study, User user, int count = 1, Filter filter = Filter.Remaining, StudyTask.Type type = StudyTask.Type.Both)
        {
            switch (filter)
            {
                case Filter.Remaining:
                    return GetRemainingTasks(study, user).
                        Where(t => t.TaskType == type).
                        Take(count).
                        Select(task => new TaskRequestDTO(task, user.Id));
                case Filter.Done:
                    return GetFinishedTasks(study, user).
                        Where(t => t.TaskType == type).
                        Take(count).
                        Select(task => new TaskRequestDTO(task, user.Id));
                case Filter.Editable:
                    return GetEditableTasks(study, user).
                        Where(t => t.TaskType == type).
                        Take(count).
                        Select(task => new TaskRequestDTO(task, user.Id));
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
        public IEnumerable<int> GetTaskIDs(Study study, User user, Filter filter = Filter.Editable, StudyTask.Type type = StudyTask.Type.Both)
        {
            switch (filter)
            {
                case Filter.Remaining:
                    return GetRemainingTasks(study, user).Where(t => t.TaskType == type).Select(t => t.Id);
                case Filter.Done:
                    return GetFinishedTasks(study, user).Where(t => t.TaskType == type).Select(t => t.Id);
                case Filter.Editable:
                    return GetEditableTasks(study, user).Where(t => t.TaskType == type).Select(t => t.Id);
                default:
                    throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }

        private static IEnumerable<StudyTask> GetFinishedTasks(Study study, User user)
        {
            return study.Stages.SelectMany(stage => stage.Tasks.Where(t => t.UserIDs.Contains(user.Id)).Where(t => !t.IsEditable));
        }

        private static IEnumerable<StudyTask> GetRemainingTasks(Study study, User user)
        {
            return study.Stages.SelectMany(stage => stage.Tasks.Where(t => t.UserIDs.Contains(user.Id)).Where(t => !t.IsFinished(user.Id)));
        }

        private static IEnumerable<StudyTask> GetEditableTasks(Study study, User user)
        {
            return study.Stages.SelectMany(stage => stage.Tasks.Where(t => t.UserIDs.Contains(user.Id)).Where(t => t.IsEditable));
        }

        /// <summary>
        /// Get the taskRequestDTO for a user
        /// </summary>
        /// <param name="userId">The userId</param>
        /// <param name="taskId">The taskId</param>
        /// <returns></returns>
        public TaskRequestDTO GetTask(int userId, int taskId)
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

            return new TaskRequestDTO(task, userId);
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
        private bool TaskMeetsStageCriteria(ICollection<Criteria> criteria, StudyTask task)
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

        /// <summary>
        /// Generates the tasks and distributes them when a new study is created or updated in the db.
        /// </summary>
        /// <param name="study"></param>
        public void OnNext(Study study)
        {
            var currentStage = study.Stages.Find(s => s.Id.Equals(study.CurrentStageID));

            //Generate the tasks for the currentstage
            var tasks = study.Items.Select(item => _taskGenerator.GenerateReviewTask(item, currentStage));

            //Find the users that are reviewers for this stage.
            var reviewers = currentStage.Users.Where(u => u.StudyRole == UserStudies.Role.Reviewer).Select(u => u.User).ToList();

            //Distribute the task to the reviewers
            tasks = _taskDistributor.Distribute(currentStage, reviewers, tasks);

            //Save the stages in the db
            foreach (var task in tasks)
            {
                _storageManager.CreateTask(task);
            }
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public virtual void Subscribe(IObservable<Study> provider)
        {
            _unsubscriber = provider.Subscribe(this);
        }


        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
    }
}
