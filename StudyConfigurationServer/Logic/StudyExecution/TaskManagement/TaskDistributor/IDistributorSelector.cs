using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement.TaskDistributor
{
    public interface IDistributorSelector
    {
        /// <summary>
        ///     Distributes the tasks to the users by finding the correct IDistributer and distribute using that.
        /// </summary>
        /// <param name="distributionRule"></param>
        /// <param name="users">The users to distribute the tasks to</param>
        /// <param name="tasks">The tasks to distribute</param>
        IEnumerable<StudyTask> Distribute(Stage.Distribution distributionRule, IEnumerable<User> users,
            IEnumerable<StudyTask> tasks);
    }
}