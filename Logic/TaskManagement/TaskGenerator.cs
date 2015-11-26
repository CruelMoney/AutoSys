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

        public IEnumerable<TaskRequest> GenerateTasks(Study study, Stage stage)
        {
            throw new NotImplementedException();
            if (stage.StageType == TaskRequest.Type.Review)
            {
                foreach (var item in study.Items)
                {
                   ///create task
                }
            }
        }

    }
}
