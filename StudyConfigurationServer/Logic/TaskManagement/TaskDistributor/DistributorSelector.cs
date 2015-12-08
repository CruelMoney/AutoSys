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
        /// 
        /// </summary>
        /// <param name="stage"></param>
        public IEnumerable<StudyTask> Distribute(Stage stage, IEnumerable<StudyTask> tasks)
        {
            var criteria = stage.Criteria;
            var distributionRule = stage.DistributionRule;
            var users = stage.Users.Select(u=>u.User).ToList();
            if (_distributors.ContainsKey(distributionRule))
            {
               return _distributors[distributionRule].Distribute(users, tasks, criteria);
            }
            else
            {
               return _defaultDistributor.Distribute(users, tasks, criteria);
            }
        }
    }

   
}