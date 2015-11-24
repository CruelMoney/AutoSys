using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Model;
using Logic.Model.DTO;

namespace Logic.TaskManagement
{
    public class TaskController
    {
        private readonly TaskDeliver _taskDeliver;
        private TaskRequester _taskRequester;

        public TaskController(TaskDeliver taskDeliver, TaskRequester taskRequester)
        {
            _taskDeliver = taskDeliver;
            _taskRequester = taskRequester;
        }

        public TaskController()
        {
            _taskDeliver = new TaskDeliver();
            _taskRequester = new TaskRequester();
        }

        public void deliverTask(TaskSubmission task)
        {
            _taskDeliver.DeliverTask(task);
        }

        public TaskRequest GetTasksForUser(User user, Study study)
        {
            
        }
    }
}
