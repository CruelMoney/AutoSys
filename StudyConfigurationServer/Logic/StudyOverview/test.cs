using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StudyOverview
{
    public class test
    {
        StudyStorageManager Storage = new StudyStorageManager();

        public Dictionary<int,int> getOverview(Stage stage)
        {

            var completedTasks = new ConcurrentDictionary<int, int>();

                foreach (var task in stage.Tasks)
                {
                    if (!task.IsDeliverable)
                    foreach (var user in task.RequestedData)
                    {
                        completedTasks.AddOrUpdate(user.Id, 1, (id, count) => count + 1);
                    }
                }

            return completedTasks.ToDictionary(k => k.Key, k => k.Value);
        }
    }
}