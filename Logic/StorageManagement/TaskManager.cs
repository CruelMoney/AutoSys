using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.StorageManagement
{
    public class TaskManager
    {
        IRepository _taskRepo;
        public TaskManager()
        {
        }

        public TaskManager(IRepository repo)
        {
            _taskRepo = repo;
        }
    }
}
