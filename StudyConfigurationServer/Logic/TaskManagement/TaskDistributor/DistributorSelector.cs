using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.CriteriaValidator;

namespace StudyConfigurationServer.Logic.TaskManagement.TaskDistributor
{
    public class DistributorSelector
    {
        readonly Dictionary<Stage.Distribution, IDistributor> _distributors;
        readonly IDistributor _defaultDistributor = new EqualDistributor();

        ///  <summary>
        /// 
        ///  </summary>
        public DistributorSelector(Dictionary<Stage.Distribution, IDistributor> distributors = null)
        {
            throw new NotImplementedException();
            _distributors = distributors ?? new Dictionary<Stage.Distribution, IDistributor>()
            {

            };
        }

        /// <summary>
        /// Distributes the tasks to the users by finding the correct IDistributer and distribute using that.
        /// </summary>
        /// <param name="stage">The stage within the tasks are distributed</param>
        /// <param name="users">The users to distribute the tasks to</param>
        /// <param name="tasks">The tasks to distribute</param>
        public IEnumerable<StudyTask> Distribute(Stage stage, ICollection<User> users, IEnumerable<StudyTask> tasks)
        {
            var distributionRule = stage.DistributionRule;
            if (_distributors.ContainsKey(distributionRule))
            {
               return _distributors[distributionRule].Distribute(users, tasks);
            }
            else
            {
               return _defaultDistributor.Distribute(users, tasks);
            }
        }
    }

   
}