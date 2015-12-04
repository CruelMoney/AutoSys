using System.Collections.Generic;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TaskManagement
{
    public class TaskController
    {
        private readonly TaskDeliver _taskDeliver;
        private TaskRequester _taskRequester;
        private StudyStorageManager _studyStorageManager;

        public TaskController(TaskDeliver taskDeliver, TaskRequester taskRequester, StudyStorageManager taskStorage)
        {
            _taskDeliver = taskDeliver;
            _taskRequester = taskRequester;
            _studyStorageManager = taskStorage;
            
        }

        public TaskController()
        {
            _taskDeliver = new TaskDeliver();
            _taskRequester = new TaskRequester();
            _studyStorageManager = new StudyStorageManager();
        }

        public void deliverTask(TaskSubmissionDTO task)
        {
            //TaskDeliver skal have kode til at ændre en submitted StudyTask til dens tilsvarende requested StudyTask, den kalder på
            // taskdeliver, som ændrer, returnerer den nye (og færdige) udgave af taskrequest. den sendes med koden herunder.
  
        }

        public List<TaskRequestDTO> GetTasksForUser(int id, int userId, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            var study = _studyStorageManager.GetStudy(id);
            TaskRequester requester = new TaskRequester();
            return requester.GetTasksForUser(userId, study, count, filter, type);

        }
    }
}
