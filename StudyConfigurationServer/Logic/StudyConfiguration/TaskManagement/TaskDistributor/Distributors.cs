using System.Collections.Generic;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.TaskDistributor
{
    /// <summary>
    /// A Distributor that gives each user each of the tasks. The users share 100% of the tasks. 
    /// </summary>
    public class EqualDistributor : IDistributor
    {
        public IEnumerable<StudyTask> Distribute(ICollection<User> users, IEnumerable<StudyTask> tasks)
        {         
            foreach (var task in tasks)
            {
                foreach (var user in users)
                {

                    task.Users.Add(user);

                    foreach (var dataField in task.DataFields)
                    {
                        dataField.UserData.Add(new UserData() {UserID = user.ID});
                    }
                  
                   }
                yield return task;
            }
            }
    }
}