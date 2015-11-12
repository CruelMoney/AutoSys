using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Storage.Repository;

namespace Logic.TaskManagement
{
    public class TaskGenerator
    {
        private readonly IRepository _repository;
        public TaskGenerator(IRepository repo, )
        {
            _repository = repo;
        }
    }
}
