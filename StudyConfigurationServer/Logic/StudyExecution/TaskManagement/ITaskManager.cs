using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.StudyExecution.TaskManagement
{
    public interface ITaskManager
    {
        /// <summary>
        /// Deliver a given taskDTO
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        bool DeliverTask(int taskId, TaskSubmissionDto task);

        /// <summary>
        ///     Generates a validating task from the conflicting tasks datafields.
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns>itemIDs to be excluded from study</returns>
        IEnumerable<StudyTask> GenerateValidationTasks(IEnumerable<StudyTask> reviewTasks);

        /// <summary>
        /// Generate review tasks for the given items according to the criteria
        /// </summary>
        /// <param name="items"></param>
        /// <param name="criteria"></param>
        /// <returns></returns>
        IEnumerable<StudyTask> GenerateReviewTasks(IEnumerable<Item> items, List<Criteria> criteria);

        /// <summary>
        /// Distribute the tasks to the given users according to the rule.
        /// </summary>
        /// <param name="users">The users to distribute to</param>
        /// <param name="distributionRule">The rule for distribution</param>
        /// <param name="tasks">The tasks to distribute</param>
        /// <returns></returns>
        IEnumerable<StudyTask> Distribute(IEnumerable<User> users, Stage.Distribution distributionRule,
            IEnumerable<StudyTask> tasks);


        /// <summary>
        ///        If the task does not contain conflicting and it doesn't fulfill the criteria data we return it's item.
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns>itemIDs to be excluded from study</returns>
        IEnumerable<Item> GetExcludedItems(ICollection<StudyTask> tasks, ICollection<Criteria> criteria);

        /// <summary>
        ///     Get the taskRequestDTO for a user.
        /// </summary>
        /// <returns></returns>
        IEnumerable<TaskRequestDto> GetTasksDtOs(ICollection<FieldType> visibleFields, List<int> taskIDs,
            int userId, int count, TaskRequestDto.Filter filter, TaskRequestDto.Type type);

        /// <summary>
        ///     Get all the taskIDs for a user
        /// </summary>
        /// <returns></returns>
        IEnumerable<int> GetTasksIDs(List<int> taskIDs, int userId, TaskRequestDto.Filter filter,
            TaskRequestDto.Type type);


        int CreateTask(StudyTask task);
        /// <summary>
        ///     Returns the resource with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the study this resource is part of.</param>
        /// <param name="resourceId">The ID of the requested resource.</param>
        ResourceDto GetResource(int id, int resourceId);

       

        /// <summary>
        /// Check if the all the users has data entered for the task. 
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        bool TaskIsFinished(int taskId);

        TaskRequestDto GetTaskDto(int taskId, int? userId = null);
    }
}