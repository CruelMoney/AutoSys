using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.TaskDistributor
{
    /// <summary>
    /// A Distributor that gives each user each of the tasks. The users share 100% of the tasks. 
    /// </summary>
    public class EqualDistributor : IDistributor
    {
        public IEnumerable<StudyTask> Distribute(IEnumerable<User> users, IEnumerable<StudyTask> tasks)
        {         
            foreach (var task in tasks)
            {
              
                    task.DataFields.ForEach(d => d.UserData.Clear());
                    task.Users.AddRange(users);

                    foreach (var user in users)
                    {
                        foreach (var dataField in task.DataFields)
                        {
                            dataField.UserData.Add(new UserData() { UserID = user.ID });
                        }

                    }
                    yield return task;
                
            }
            }
    }

    public class NoOverlapDistributor : IDistributor
    {
        public IEnumerable<StudyTask> Distribute(IEnumerable<User> users, IEnumerable<StudyTask> tasks)
        {

            var userList = users.ToList();
            var sublists = new List<List<StudyTask>>();

            int rangeSize = tasks.Count() / userList.Count();
            int additionalItems = tasks.Count() % userList.Count();
            int index = 0;

            while (index < tasks.Count())
            {
                int currentRangeSize = rangeSize + ((additionalItems > 0) ? 1 : 0);
                sublists.Add(tasks.ToList().GetRange(index, currentRangeSize));
                index += currentRangeSize;
                additionalItems--;
            }

            int ui = 0;

            foreach (var sublist in sublists)
            {
                foreach (var task in sublist)
                {
                        task.DataFields.ForEach(d=>d.UserData.Clear());
                        task.Users.Add(userList[ui]);
                        foreach (var dataField in task.DataFields)
                        {
                            dataField.UserData.Add(new UserData() { UserID = userList[ui].ID });
                        }

                    
                    yield return task;
                }
                ui++;
            }
            
        }
    }
}