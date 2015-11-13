using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.StorageManagement
{
    public class TaskStorageManager
    {
        IRepository _taskRepo;
        public TaskStorageManager()
        {
        }

        public TaskStorageManager(IRepository repo)
        {
            _taskRepo = repo;
        }
    }
}
