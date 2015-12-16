#region Using

using System;
using System.Collections.Generic;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement.TaskDistributor
{
    /// <summary>
    /// Chooses a distributer based on the distributionRule enum. 
    /// </summary>
    public class DistributorSelector : IDistributorSelector
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
        /// Distribute using the matched distributor. 
        /// </summary>
        /// <param name="distributionRule">The rule for distribution.</param>
        /// <param name="users">The users to distribute to</param>
        /// <param name="tasks">The tasks to distribute</param>
        /// <returns></returns>
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