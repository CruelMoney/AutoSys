using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logic.Data;

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

        public void deliverTask(UserTask task)
        {
            _taskDeliver.DeliverTask(task);
        }

        public void getTaskForUser(User user, Study study)
        {
            _taskRequester.GetTaskForUser(user, study);
        }
    }
}
