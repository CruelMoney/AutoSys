using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Logic.TaskManagement.TaskDistributor
{
    public interface IDistributor
    {
        IEnumerable<StudyTask> Distribute(Stage stage, IEnumerable<StudyTask> tasks);
    }
}