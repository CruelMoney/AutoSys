using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Ajax.Utilities;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.TaskManagement.TaskDistributor;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.CriteriaValidator;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TaskManagement
{
    public class TaskController : IObserver<Study>
    {
        private DistributorSelector _taskDistributor;
        private TaskGenerator _taskGenerator;
        private TaskRequester _taskRequester;
        private TaskStorageManager _storageManager;
        private StudyStorageManager _studyStorage;
        private CriteriaValidator _criteriaValidator;

        private IDisposable _unsubscriber;

        public TaskController(TaskRequester taskRequester, StudyStorageManager taskStorage, TaskGenerator taskGenerator, TaskStorageManager storageManager)
        {
            _taskRequester = taskRequester;
            _taskGenerator = taskGenerator;
            _storageManager = storageManager;
        }

        public TaskController()
        {
            _taskGenerator = new TaskGenerator();
            _storageManager = new TaskStorageManager();
            _taskRequester = new TaskRequester();
        }


        public void DeliverTask(int taskID, TaskSubmissionDTO task)
        {
            var taskToUpdate = _storageManager.GetTask(taskID);

            if (!taskToUpdate.IsEditable)
            {
                throw new InvalidOperationException("The task is not editable");
            }

            taskToUpdate.SubmitData(task);
            _storageManager.UpdateTask(taskToUpdate);



            var currentStage = taskToUpdate.Stage;

            if (taskToUpdate.TaskType == StudyTask.Type.Review)
            {
                if (currentStage.IsFinished())
                {
                    foreach (var studyTask in currentStage.Tasks)
                    {
                        studyTask.IsEditable = false;

                        throw new NotImplementedException("Create validation tasks if task has conflicting data");
                        throw new NotImplementedException("CriteriaValidate the task if no conflicting data");

                        _storageManager.UpdateTask(studyTask);
                    }
                }
            }

            if (taskToUpdate.TaskType == StudyTask.Type.Conflict)
            {
                if (currentStage.IsFinished())
                {
                    foreach (var studyTask in currentStage.Tasks)
                    {
                        studyTask.IsEditable = false;

                        if (TaskMeetsStageCriteria(currentStage, studyTask))
                        {
                            currentStage.Study.Items.Remove(studyTask.Paper);
                        }

                        _storageManager.UpdateTask(studyTask);

                    }

                    currentStage.Study.MoveToNextStage();

                }
            }
        }

        public List<TaskRequestDTO> GetTasksForUser(int id, int userId, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
           throw new NotImplementedException();
        }

        /// <summary>
        /// Checks if a task meets the stages various criteria.
        /// Each of the tasks fields are checked against the corresponding criteria.
        /// </summary>
        /// <param name="stage">The stage we are checking for</param>
        /// <param name="task">The task we are checking</param>
        /// <returns></returns>
        public bool TaskMeetsStageCriteria(Stage stage, StudyTask task)
        {
            if (stage.Criteria.Count==0)
            {
                 throw new ArgumentException("the stage does not contain any criteria");
            }

            foreach (var criterion  in stage.Criteria)
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
            var tasks = study.Items.Select(item => _taskGenerator.GenerateReviewTask(item, currentStage));
            tasks = _taskDistributor.Distribute(currentStage, tasks);

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
