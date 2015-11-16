using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Data;
using Logic.StorageManagement;

namespace Logic.TaskManagement
{
    public class TaskDeliver
    {
        private TaskStorageManager _storageManager;

    

        public TaskDeliver(TaskStorageManager storageManager)
        {
            _storageManager = storageManager;
        }

        public TaskDeliver()
        {
            _storageManager = new TaskStorageManager();
        }

        public void DeliverTask(UserTask taskToDeliver)
        {
            var task = _storageManager.FindStoredTask(taskToDeliver);
            _storageManager.SaveTask(task); 
        }


    }
}
