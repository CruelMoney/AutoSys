using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model;
using Logic.Model.DTO;
using Logic.StorageManagement;
using Storage;
using Storage.Repository;

namespace Logic.TaskManagement
{
    public class TaskGenerator
    {
        public TaskGenerator(StudyLogic study)
        {
         
        }

        public IEnumerable<TaskRequest> GenerateTasks(StudyLogic study, StageLogic stage)
        {
            throw new NotImplementedException();
           
        }

    }
}
