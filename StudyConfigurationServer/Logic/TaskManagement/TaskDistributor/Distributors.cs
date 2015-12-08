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
        public IEnumerable<StudyTask> Distribute(List<User> users, IEnumerable<StudyTask> tasks, Criteria criteria)
        {         
                foreach (var task in tasks)
                {
                foreach (var user in users)
                {
                    if (task.DataFields.Select(d => d.UserData.Where(u => u.User.Id == user.Id)).Any())
                    {
                        throw new ArgumentException("The user is already connected to the task");
                    }

                    foreach (var dataField in task.DataFields)
                    {
                        dataField.UserData.Add(new UserData() {User = user});
                    }
                        
                    yield return task;
                    }
                }
            }
        

    }
}