#region Using

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

#endregion

namespace StudyConfigurationServer.Models.DTO
{
    /// <summary>
    ///     A requested StudyTask, part of a systematic study.
    /// </summary>
    public class TaskRequestDto
    {
        /// <summary>
        ///     Filters StudyTask requests.
        /// </summary>
        public enum Filter
        {
            /// <summary>
            ///     Only list remaining tasks.
            /// </summary>
            Remaining,

            /// <summary>
            ///     Only list delivered tasks which are still editable.
            /// </summary>
            Editable,

            /// <summary>
            ///     Only list tasks which are done, and are no longer editable.
            /// </summary>
            Done
        }

        /// <summary>
        ///     Defines whether the requested tasks are reviewing tasks, conflict tasks, or any StudyTask.
        /// </summary>
        public enum Type
        {
            Both,
            Review,
            Conflict
        }

        public TaskRequestDto()
        {
        }

        public TaskRequestDto(StudyTask task, ICollection<FieldType> visibleFieldTypes, int? userId = null)
        {
            var editableFields = new List<DataFieldDto>();

            foreach (var dataField in task.DataFields)
            {
                editableFields.Add(new DataFieldDto(dataField, userId));
            }

            var visibleFields = new List<DataFieldDto>();

            foreach (var dataType in visibleFieldTypes)
            {
                visibleFields.Add(new DataFieldDto(dataType, task.Paper));
            }

            IsDeliverable = task.IsEditable;
            TaskType = (Type) Enum.Parse(typeof (Type), task.TaskType.ToString());
            Id = task.ID;
            RequestedFields = editableFields.ToArray();
            VisibleFields = visibleFields.ToArray();

            if (task.TaskType == StudyTask.Type.Conflict)
            {
                //Creating the conflicting Data
                var conflictinData = new ConflictingDataDto[task.DataFields.Count][];

                //For each dataField
                for (var d = 0; d < task.DataFields.Count; d++)
                {
                    //Create new conflicting data
                    conflictinData[d] = new ConflictingDataDto[task.Users.Count];

                    //Add each users data and id to the conflicting data
                    for (var u = 0; u < task.Users.Count; u++)
                    {
                        var userData = task.DataFields[d].ConflictingData[u];
                        conflictinData[d][u] = new ConflictingDataDto
                        {
                            Data = userData.Data.Select(s => s.Value).ToArray(),
                            UserId = userData.UserId
                        };
                    }
                }

                ConflictingData = conflictinData;
            }
        }

        /// <summary>
        ///     A unique identifier for the StudyTask.
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        ///     The <see cref="Type" /> of the StudyTask, either a review StudyTask, or a conflict StudyTask.
        /// </summary>
        [Required]
        public Type TaskType { get; set; }

        /// <summary>
        ///     Defines whether the StudyTask is still deliverable or not.
        /// </summary>
        [Required]
        public bool IsDeliverable { get; set; }

        /// <summary>
        ///     A list of data fields which are to be shown to the User, but are not editable.
        /// </summary>
        [Required]
        public DataFieldDto[] VisibleFields { get; set; }

        /// <summary>
        ///     A list of requested data fields which need to be filled out as part of the StudyTask.
        /// </summary>
        [Required]
        public DataFieldDto[] RequestedFields { get; set; }

        /// <summary>
        ///     In case this is a <see cref="Type.Conflict" /> StudyTask, represents for each of the
        ///     <see cref="RequestedFields" /> the list of <see cref="ConflictingData" /> provided by separate users.
        /// </summary>
        public ConflictingDataDto[][] ConflictingData { get; set; }
    }
}