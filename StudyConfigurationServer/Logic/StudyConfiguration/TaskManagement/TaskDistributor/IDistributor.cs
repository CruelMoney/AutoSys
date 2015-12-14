using System.Collections.Generic;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement.TaskDistributor
{
    public interface IDistributor
    {
        IEnumerable<StudyTask> Distribute(IEnumerable<User> users, IEnumerable<StudyTask> tasks);
    }
}