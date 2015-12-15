#region Using

using System;
using System.Collections.Generic;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.TaskDistributor
{
    public class DistributorSelector
    {
        private readonly IDistributor _defaultDistributor = new EqualDistributor();
        private readonly Dictionary<Stage.Distribution, IDistributor> _distributors;

        /// <summary>
        /// </summary>
        public DistributorSelector(Dictionary<Stage.Distribution, IDistributor> distributors = null)
        {
            _distributors = distributors ?? new Dictionary<Stage.Distribution, IDistributor>
            {
                {Stage.Distribution.HundredPercentOverlap, new EqualDistributor()},
                {Stage.Distribution.NoOverlap, new NoOverlapDistributor()}
            };
        }

        /// <summary>
        ///     Distributes the tasks to the users by finding the correct IDistributer and distribute using that.
        /// </summary>
        /// <param name="stage">The stage within the tasks are distributed</param>
        /// <param name="users">The users to distribute the tasks to</param>
        /// <param name="tasks">The tasks to distribute</param>
        public IEnumerable<StudyTask> Distribute(Stage.Distribution distributionRule, IEnumerable<User> users,
            IEnumerable<StudyTask> tasks)
        {
            if (_distributors.ContainsKey(distributionRule))
            {
                return _distributors[distributionRule].Distribute(users, tasks);
            }
            throw new NotImplementedException("The distributer is not implemented");
        }
    }
}