#region Using

using System;
using System.Collections.Generic;
using StudyConfigurationServer.Models;

#endregion

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement.TaskDistributor
{
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