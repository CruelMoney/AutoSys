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
        public TaskGenerator(Study study)
        {
         
        }

        public IEnumerable<TaskRequestDTO> GenerateTasks(Study study, Stage stage)
        {
            throw new NotImplementedException();
           
        }

    }
}
