using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Data;
using Storage;
using Storage.Repository;

namespace Logic.TaskManagement
{
    public class TaskGenerator
    {
        private readonly IRepository _repository;
        private readonly User _user;
        private readonly Study _study;

        public TaskGenerator(IRepository repo, Study study, User user)
        {
            _repository = repo;
            _study = study;
            _user = user;
        }
    }
}
