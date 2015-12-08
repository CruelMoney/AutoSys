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
            var distributionRule = stage.DistributionRule;
            if (_distributors.ContainsKey(distributionRule))
            {
               return _distributors[distributionRule].Distribute(stage, tasks);
            }
            else
            {
               return _defaultDistributor.Distribute(stage, tasks);
            }
        }
    }

   
}