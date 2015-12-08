using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.TaskManagement.TaskDistributor
{
    /// <summary>
    /// A Distributor that gives each user each of the tasks. The users share 100% of the tasks. 
    /// </summary>
    public class EqualDistributor : IDistributor
    {
        public IEnumerable<StudyTask> Distribute(Stage stage, IEnumerable<StudyTask> tasks)
        {         
                foreach (var task in tasks)
                {
                foreach (var user in stage.Users.Select(u => u.User))
                {
                    //For each user create a new requestedData relation for the task.
                    if (!task.RequestedData.Select(t => t.User).Contains(user))
                    {
                        var requestedData = new TaskRequestedData()
                        {
                            User = user,
                            IsDeliverable = true,
                            IsFinished = false,
                        };

                        //For each criteria add a field to fill out
                        foreach (var criterion in stage.Criteria)
                        {
                            var field = new DataField()
                            {
                                Name = criterion.Name,
                                Description = criterion.Description,
                                FieldType = criterion.DataType,
                            };

                           requestedData.Data.Add(field);
                        }

                        //Add the requested data to the task.
                        task.RequestedData.Add(requestedData);
                        yield return task;
                    }
                }
            }
        }
    }
}