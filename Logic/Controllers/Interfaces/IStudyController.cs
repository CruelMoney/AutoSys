using System;
using System.Collections.Generic;
using System.Web.Http;
using Logic.Model.DTO;

namespace Logic.Controllers.Interfaces
{
   
    public interface IStudyController
    {
        /// <summary>
        /// Retrieve an overview of the specified study as <see cref="StudyOverviewDTO"/>.
        /// </summary>
        /// <param name="id">The ID of the study for which to retrieve an overview.</param>
        [Route("{id}/Overview")]
        IHttpActionResult GetOverview(int id);

        /// <summary>
        /// Get requested tasks for a specific user of a given study. By default, the first remaining (still to be completed) StudyTask is retrieved.
        /// Optionally, the amount of tasks to retrieve, and the type of tasks to retrieve are specified.
        /// </summary>
        /// <param name="id">The ID of the study to get tasks for.</param>
        /// <param name="userId">The ID of the user to get tasks for.</param>
        /// <param name="count">The amount of tasks to retrieve.</param>
        /// <param name="filter">Defines whether to get remaining tasks, delivered (but still editable) tasks, or completed tasks.</param>
        /// <param name="type">The type of tasks to retrieve.</param>
        [Route("{id}/StudyTask")]
        IHttpActionResult GetTasks(int id, int userId, int count = 1, TaskRequestDTO.Filter filter = TaskRequestDTO.Filter.Remaining, TaskRequestDTO.Type type = TaskRequestDTO.Type.Both);

        /// <summary>
        /// Get requested StudyTask IDs for a specific user of a given study. By default, delivered but still editable StudyTask IDs are returned.
        /// Optionally, the type of StudyTask IDs to retrieve are specified.
        /// </summary>
        /// <param name="id">The ID of the study to get tasks for.</param>
        /// <param name="userId">The ID of the user to get tasks for.</param>
        /// <param name="filter">Defines whether to get remaining tasks, delivered (but still editable) tasks, or completed tasks.</param>
        /// <param name="type">The type of tasks to retrieve.</param>
        [Route("{id}/TaskIDs")]
        IHttpActionResult GetTaskIDs(int id, int userId, TaskRequestDTO.Filter filter = TaskRequestDTO.Filter.Editable,
            TaskRequestDTO.Type type = TaskRequestDTO.Type.Both);

        /// <summary>
        /// Get a requested StudyTask with a specific ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [Route("{id}/StudyTask/{taskId}")]
        IHttpActionResult GetTask(int id, int taskId);

        /// <summary>
        /// Deliver a finished StudyTask.
        /// A StudyTask can be redelivered as long as it is editable.
        /// Which tasks are editable can be found by calling <see cref="GetTaskIDs" /> with filter set to <see cref="TaskRequestDTO.Filter.Editable" />.
        /// An error is returned in case the StudyTask can no longer be delivered.
        /// </summary>
        /// <param name="id">The ID of the study the StudyTask is part of.</param>
        /// <param name="taskId">The ID of the StudyTask.</param>
        /// <param name="task">The completed StudyTask.</param>
        [Route("{id}/StudyTask/{taskId}")]
        IHttpActionResult PostTask(int id, int taskId, [FromBody] TaskSubmissionDTO task);

        /// <summary>
        /// Returns the resource with the specified ID.
        /// The resource is returned as StreamContent.
        /// </summary>
        /// <param name="id">The ID of the study this resource is part of.</param>
        /// <param name="resourceId">The ID of the requested resource.</param>
        [Route("{id}/ResourceDTO/{resourceId}")]
        IHttpActionResult GetResource(int id, int resourceId);
    }
}
