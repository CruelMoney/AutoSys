using System;
using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.TaskManagement.TaskDistributor;
using StudyConfigurationServer.Models;
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
            taskToUpdate.SubmitData(task);
            _storageManager.UpdateTask(taskToUpdate);
  
            if (taskToUpdate.Stage.IsFinished())
            {
                var study = taskToUpdate.Stage.Study;
                study.MoveToNextStage();
                _studyStorage.UpdateStudy(study);
            }
        }

        public List<TaskRequestDTO> GetTasksForUser(int id, int userId, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
           throw new NotImplementedException();
        }

        /// <summary>
        /// Generates the tasks and distributes them when a new study is created or updated in the db.
        /// </summary>
        /// <param name="study"></param>
        public void OnNext(Study study)
        {
           var currentStage = study.Stages.Find(s => s.Id.Equals(study.CurrentStageID));
           var tasks = _taskDistributor.Distribute(currentStage, _taskGenerator.GenerateTasks(currentStage, study.Items));

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
