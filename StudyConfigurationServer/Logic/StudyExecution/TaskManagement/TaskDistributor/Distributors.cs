#region Using

using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement.TaskDistributor
{
    /// <summary>
    ///     A Distributor that gives each user each of the tasks. The users share 100% of the tasks.
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
                        dataField.UserData.Add(new UserData {UserId = user.ID});
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

            var rangeSize = tasks.Count()/userList.Count();
            var additionalItems = tasks.Count()%userList.Count();
            var index = 0;

            while (index < tasks.Count())
            {
                var currentRangeSize = rangeSize + (additionalItems > 0 ? 1 : 0);
                sublists.Add(tasks.ToList().GetRange(index, currentRangeSize));
                index += currentRangeSize;
                additionalItems--;
            }

            var ui = 0;

            foreach (var sublist in sublists)
            {
                foreach (var task in sublist)
                {
                    task.DataFields.ForEach(d => d.UserData.Clear());
                    task.Users.Add(userList[ui]);
                    foreach (var dataField in task.DataFields)
                    {
                        dataField.UserData.Add(new UserData {UserId = userList[ui].ID});
                    }


                    yield return task;
                }
                ui++;
            }
        }
    }
}