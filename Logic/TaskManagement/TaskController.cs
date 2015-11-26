using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model;
using Logic.Model.DTO;
using Logic.StorageManagement;


namespace Logic.TaskManagement
{
    public class TaskController
    {
        private readonly TaskDeliver _taskDeliver;
        private TaskRequester _taskRequester;
        private StudyStorageManager _studyStorageManager;

        public TaskController(TaskDeliver taskDeliver, TaskRequester taskRequester, StudyStorageManager taskStorage)
        {
            _taskDeliver = taskDeliver;
            _taskRequester = taskRequester;
            _studyStorageManager = taskStorage;
            
        }

        public TaskController()
        {
            _taskDeliver = new TaskDeliver();
            _taskRequester = new TaskRequester();
            _studyStorageManager = new StudyStorageManager();
        }

        public void deliverTask(TaskSubmission task)
        {
            //TaskDeliver skal have kode til at ændre en submitted task til dens tilsvarende requested task, den kalder på
            // taskdeliver, som ændrer, returnerer den nye (og færdige) udgave af taskrequest. den sendes med koden herunder.
  
        }

        public TaskRequest GetTasksForUser(int id, int userId, int count, TaskRequest.Filter filter, TaskRequest.Type type)
        {
            var study = _studyStorageManager.GetStudy(id);
            StageLogic currentStageLogic;
            foreach(var stage in study.Stages)
            {
                if (stage.Id == study.CurrentStage)
                {
                    currentStageLogic = stage;
                }
            }
            var users = study.Team.Users;
            UserLogic currentUser;
            foreach(var user in users)
            {
                if (user.Id == userId)
                {
                    currentUser = user;
                }
            }
            List<TaskLogic> tasks;
            currentStageLogic.UserTasks.TryGetValue(currentUser, out tasks);
           
        }
    }
}
