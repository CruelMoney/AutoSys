using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Data;
using Logic.StorageManagement;
using Storage;
using Storage.Repository;

namespace Logic.TaskManagement
{
    public class TaskGenerator
    {

        Study _study;

        public TaskGenerator(Study study)
        {
            _study = study;
        }

        public void generateTasks()
        {
            
        }

    }
}
