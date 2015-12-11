using System.Collections.Generic;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.TaskDistributor
{
    public interface IDistributor
    {
        IEnumerable<StudyTask> Distribute(ICollection<User> users, IEnumerable<StudyTask> tasks);
    }
}