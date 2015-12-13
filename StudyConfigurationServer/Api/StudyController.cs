﻿using System;
using System.Net;
using System.Reflection;
using System.Web.Http;
using StudyConfigurationServer.Api.Interfaces;
using StudyConfigurationServer.Logic.StudyConfiguration;
using StudyConfigurationServer.Models.DTO;
using StudyConfigurationServer.Logic.StudyOverview;

namespace StudyConfigurationServer.Api
{
    /// <summary>
    /// Controller to access information about a study.
    /// </summary>
    [RoutePrefix("api/Study")]
    public class StudyController : ApiController
    {
        StudyOverviewController controller = new StudyOverviewController();
        StudyManager _studyManager = new StudyManager();

        /// <summary>
        /// Retrieve an overview of the specified study.
        /// </summary>
        /// <param name="id">The ID of the study for which to retrieve an overview.</param>
        [Route("{id}/Overview")]
        public IHttpActionResult GetOverview(int id)
        {
            // GET: api/Study/5/Overview
            try
            {
                return Ok(controller.GetOverview(id));
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(NullReferenceException))
                {
                    return NotFound();
                }
                
                throw;
            }
           
         
        }
        

        /// <summary>
        /// Get requested tasks for a specific User of a given study. By default, the first remaining (still to be completed) StudyTask is retrieved.
        /// Optionally, the amount of tasks to retrieve, and the type of tasks to retrieve are specified.
        /// </summary>
        /// <param name="id">The ID of the study to get tasks for.</param>
        /// <param name="userId">The ID of the User to get tasks for.</param>
        /// <param name="count">The amount of tasks to retrieve.</param>
        /// <param name="filter">Defines whether to get remaining tasks, delivered (but still editable) tasks, or completed tasks.</param>
        /// <param name="type">The type of tasks to retrieve.</param>
        [Route("{id}/Task")]
        public IHttpActionResult GetTasks(int id, int userId, int count = 1, TaskRequestDTO.Filter filter = TaskRequestDTO.Filter.Remaining, TaskRequestDTO.Type type = TaskRequestDTO.Type.Both)
        {
            // GET: api/Study/4/StudyTask?userId=5&count=1&filter=Remaining&type=Review

            try
            {
                return Ok(_studyManager.GetTasks(id, userId, count, filter, type));
            }
            catch (Exception)
            {
                return NotFound();
            } 


        }

        /// <summary>
        /// Get requested StudyTask IDs for a specific User of a given study. By default, delivered but still editable StudyTask IDs are returned.
        /// Optionally, the type of StudyTask IDs to retrieve are specified.
        /// </summary>
        /// <param name="id">The ID of the study to get tasks for.</param>
        /// <param name="userId">The ID of the User to get tasks for.</param>
        /// <param name="filter">Defines whether to get remaining tasks, delivered (but still editable) tasks, or completed tasks.</param>
        /// <param name="type">The type of tasks to retrieve.</param>
        [Route("{id}/TaskIDs")]
        public IHttpActionResult GetTaskIDs(int id, int userId, TaskRequestDTO.Filter filter = TaskRequestDTO.Filter.Editable, TaskRequestDTO.Type type = TaskRequestDTO.Type.Both)
        {
            // GET: api/Study/4/TaskIDs?userId=5&filter=Editable
            try
            {
                return Ok(_studyManager.GetTasksIDs(id, userId, filter, type));
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(NullReferenceException))
                {
                    return NotFound();
                }
                if (e.GetType() == typeof(ArgumentException))
                {
                    return BadRequest();
                }
                throw;
            }
        }

        /// <summary>
        /// Get a requested StudyTask with a specific ID.
        /// </summary>
        /// <param name="id">The id of the user</param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [Route("{id}/Task/{taskId}")]
        public IHttpActionResult GetTask(int id, int taskId)
        {
            // GET: api/Study/4/StudyTask/5
           return Ok(_studyManager.GetTask(taskId));

        }

        /// <summary>
        /// Deliver a finished StudyTask.
        /// A StudyTask can be redelivered as long as it is editable.
        /// Which tasks are editable can be found by calling <see cref="GetTaskIDs" /> with filter set to <see cref="TaskRequestDTO.Filter.Editable" />.
        /// An error is returned in case the StudyTask can no longer be delivered.
        /// </summary>
        /// <param name="id">The ID of the study the StudyTask is part of.</param>
        /// <param name="taskId">The ID of the StudyTask.</param>
        /// <param name="task">The completed StudyTask.</param>
        [Route("{id}/Task/{taskId}")]
        public IHttpActionResult Post(int id, int taskId, [FromBody]TaskSubmissionDTO task)
        {
            // POST: api/Study/4/StudyTask/5

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var updated = false;

            try
            {
                updated = _studyManager.DeliverTask(id, taskId, task);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(TargetException))
                {
                    return BadRequest();
                }
            }
            if (!updated)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);

            
        }

        /// <summary>
        /// Returns the resource with the specified ID.
        /// The resource is returned as StreamContent.
        /// </summary>
        /// <param name="id">The ID of the study this resource is part of.</param>
        /// <param name="resourceId">The ID of the requested resource.</param>
        [Route("{id}/Resource/{resourceId}")]
        public IHttpActionResult GetResource(int id, int resourceId)
        {
            // GET: api/Study/4/ResourceDTO/5
            throw new NotImplementedException();
        }
    }
}
