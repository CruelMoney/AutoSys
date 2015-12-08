using System;
using System.Collections.Generic;
using System.Linq;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.TaskManagement
{
    public class TaskDeliver
    {

       
        
        public StudyTask SubmitDataToTask(StudyTask task, TaskSubmissionDTO taskToDeliver)
        {
            var userID = taskToDeliver.UserId;
            var newDataFields = taskToDeliver.SubmittedFieldsDto.ToList();
            var dataToUpdate = task.RequestedData.First(u => u.User.Id.Equals(userID)).Data;

            //TODO For now we use the dataField name to update the data.
            foreach (var field in newDataFields)
            {
                dataToUpdate.First(f => f.Name.Equals(field.Name)).Data = field.Data;
            }

            //We expect that the user only exists once per requestedData
            task.RequestedData.
            First(t => t.User.Id.Equals(userID)).
            Data = dataToUpdate;

            return task;
        }


    }
}
