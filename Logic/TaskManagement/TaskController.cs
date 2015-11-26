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
        private TaskStorageManager _storageManager;

        public TaskController(TaskDeliver taskDeliver, TaskRequester taskRequester, TaskStorageManager taskStorage)
        {
            _taskDeliver = taskDeliver;
            _taskRequester = taskRequester;
            _storageManager = taskStorage;
        }

        public TaskController()
        {
            _taskDeliver = new TaskDeliver();
            _taskRequester = new TaskRequester();
            _storageManager = new TaskStorageManager();
        }

        public void deliverTask(TaskSubmission task)
        {
            //TaskDeliver skal have kode til at ændre en submitted task til dens tilsvarende requested task, den kalder på
            // taskdeliver, som ændrer, returnerer den nye (og færdige) udgave af taskrequest. den sendes med koden herunder.
  
        }

        public TaskRequest GetTasksForUser(User user, Study study)
        {
            throw new NotImplementedException();
        }
    }
}
