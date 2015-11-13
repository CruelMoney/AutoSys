using Storage.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.StorageManagement
{
    public class StudyManager
    {
        IRepository _studyRepo;
        public StudyManager()
        {
        }

        public StudyManager(IRepository repo)
        {
            _studyRepo = repo;
        }
    }
}
