using StudyConfigurationServer.Logic.StorageManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
using Microsoft.Ajax.Utilities;
using Storage.Repository;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser;
using StudyConfigurationServer.Logic.StudyConfiguration.BiblographyParser.bibTex;
using StudyConfigurationServer.Logic.StudyConfiguration.TaskManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;
using FieldType = StudyConfigurationServer.Models.FieldType;

namespace StudyConfigurationServer.Logic.StudyConfiguration
{
    public class StudyManager
    {
        private readonly StudyStorageManager _studyStorageManager;
        private readonly TeamStorageManager _teamStorage;
        private readonly TaskManager _taskManager;
        

        public StudyManager()
        {
            var repo = new EntityFrameworkGenericRepository<StudyContext>();
            _teamStorage = new TeamStorageManager(repo);
            _studyStorageManager = new StudyStorageManager(repo);
            _taskManager = new TaskManager(repo);
        }

        public StudyManager(StudyStorageManager storageManager, TaskManager taskManager, TeamStorageManager teamStorage)
        {
            _studyStorageManager = storageManager;
            _taskManager = taskManager;
            _teamStorage = teamStorage;
        }

       //TODO check if whole study finished
        public void DeliverTask(int studyID, int taskID, TaskSubmissionDTO taskDTO)
        {         
            var currentStudy = _studyStorageManager.GetAll()
                .Where(s => s.ID == studyID)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();
            
            if (currentStudy == null)
            {
                throw new NullReferenceException("Study not found");
            }
            if (!currentStudy.Team.Users.Select(u => u.ID).Contains(taskDTO.UserId))
            {
                throw new ArgumentException("The user is not part of this study");
            }

            _taskManager.DeliverTask(taskID, taskDTO);

            //Determine if the stage is finished
            if (currentStudy.CurrentStage().
                Tasks.Select(t=>t.ID).ToList().
                TrueForAll(t=>_taskManager.
                TaskIsFinished(t)))
            {
               MoveToNextPhase(currentStudy);
            }
        }

        private void MoveToNextPhase(Study study)
        {
            var currentStage = study.CurrentStage();

            switch (currentStage.CurrentTaskType)
            {
                case StudyTask.Type.Review:
                    FinishReviewPhase(study);
                    break;
                case StudyTask.Type.Conflict:
                    FinishConflictPhase(study);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _studyStorageManager.Update(study);
        }

        private void FinishReviewPhase(Study study)
        {
            var currentStage = study.CurrentStage();
            
            //Start the validation phase and check if any tasks have been generated
            var validationTasks = StartConflictPhase(study).ToList();

            if (!validationTasks.Any())
            {
                //Finish the phase if no validation tasks
                FinishConflictPhase(study);
            }

        }

        private void FinishConflictPhase(Study study)
        {
            var currentStage = study.CurrentStage();
            var tasks = currentStage.Tasks;
            var criteria = currentStage.Criteria;

            //If there's any validation tasks we validate them
            if (tasks.Any())
            {
                var excludedItems = _taskManager.GetExcludedItems(tasks, criteria);

                //Remove the excluded items from the study
                study.Items.RemoveAll(i => excludedItems.Contains(i));
            }

            //move to the next stage
            study.MoveToNextStage();
            if (!study.IsFinished)
            {
            StartReviewPhase(study);
        }
        }

        private IEnumerable<StudyTask> StartReviewPhase(Study study)
        {
            //Find the current stage
            var stage = study.CurrentStage();
            stage.CurrentTaskType = StudyTask.Type.Review;
            
            //Find the reviewers
            var reviewers =stage.Users.Where(u => u.StudyRole == UserStudies.Role.Reviewer).Select(u => u.User);

            //Generate the tasks
            var reviewTasks =_taskManager.GenerateReviewTasks(study.Items, stage.Criteria).ToList();
            
            //Autoexlcuded items that have been filled out  
            var excludedItems = _taskManager.GetExcludedItems(reviewTasks, stage.Criteria).ToList();

            //Remove the excluded items from the study, and move to next stage
            study.Items.RemoveAll(i => excludedItems.Contains(i));

            //Distribute the tasks
            var tasks = _taskManager.Distribute(reviewers, stage.DistributionRule,
                _taskManager.GenerateReviewTasks(study.Items, stage.Criteria)).ToList();

            //Set the tasks in the stage to the reviewTasks
            stage.Tasks = tasks.ToList();

            if (!tasks.Any())
            {
               FinishConflictPhase(study);
            }
            
            return tasks;
        }

        private IEnumerable<StudyTask> StartConflictPhase(Study study)
        {
            var stage = study.CurrentStage();

            var tasks = stage.Tasks.Where(t => t.ContainsConflictingData());
            var validators = stage.Users.Where(u => u.StudyRole == UserStudies.Role.Validator).Select(u => u.User);

            //Update stage to start the validations and generate the validation tasks
            stage.CurrentTaskType = StudyTask.Type.Conflict;
            var validationTasks = _taskManager.GenerateValidationTasks(tasks);

            //Always distribute validation tasks with no overlap
            var distributedTasks = _taskManager.Distribute(validators, Stage.Distribution.NoOverlap, validationTasks);

            stage.Tasks.AddRange(distributedTasks);
            
            return distributedTasks;
        }

    
        public Study ConvertStudy(StudyDTO studyDTO)
        {
            var study = new Study()
            {
                IsFinished = false,
                Name = studyDTO.Name,
                Team = _teamStorage.GetTeam(studyDTO.Team.Id),
                Items = new List<Item>(),
                Stages = new List<Stage>()
            };
            //Parse items
            var parser = new BibTexParser(new ItemValidator());
            var fileString = System.Text.Encoding.Default.GetString(studyDTO.Items);
            study.Items = parser.Parse(fileString);


            var firstStage = true;

            foreach (var stageDto in studyDTO.Stages)
            {
                var stage = CreateStage(stageDto);

                study.Stages.Add(stage);

                if (firstStage)
                {
                    stage.IsCurrentStage = true;
                }

                firstStage = false;
            }
            return study;
        }
            
        public int CreateStudy(StudyDTO studyDTO)
        {
            var study = ConvertStudy(studyDTO);
            StartReviewPhase(study);

            _studyStorageManager.Save(study);

            return study.ID;
        }

        private Stage CreateStage(StageDTO stageDto)
        {
            var stage = new Stage()
            {
                Name = stageDto.Name,
                CurrentTaskType = StudyTask.Type.Review,
                DistributionRule = (Stage.Distribution)Enum.Parse(typeof(Stage.Distribution), stageDto.DistributionRule.ToString()),
                VisibleFields = new List<FieldType>(),
                Users = new List<UserStudies>(),
                Criteria = new List<Criteria>(),
            };

            stageDto.VisibleFields.ForEach(
                f => stage.VisibleFields.Add(new FieldType(f.ToString())));

            stageDto.ReviewerIDs.ForEach(u =>
                stage.Users.Add(new UserStudies()
                {
                    StudyRole = UserStudies.Role.Reviewer,
                    User = _teamStorage.GetUser(u)
                }));

            stageDto.ValidatorIDs.ForEach(u =>
                stage.Users.Add(new UserStudies()
                {
                    StudyRole = UserStudies.Role.Validator,
                    User = _teamStorage.GetUser(u)
                }));

            var criteria = new Criteria()
            {
                Name = stageDto.Criteria.Name,
                DataMatch = stageDto.Criteria.DataMatch.Select(s => new StoredString() { Value = s }).ToArray(),
                DataType = (DataField.DataType)Enum.Parse(typeof(DataField.DataType), stageDto.Criteria.DataType.ToString()),
                Description = stageDto.Criteria.Description,
                Rule = (Criteria.CriteriaRule)Enum.Parse(typeof(Criteria.CriteriaRule), stageDto.Criteria.Rule.ToString()),
            };

            if (stageDto.Criteria.TypeInfo != null)
            {
                criteria.TypeInfo = stageDto.Criteria.TypeInfo.Select(s => new StoredString() { Value = s }).ToArray();
            }

            stage.Criteria.Add(criteria);

            return stage;
        }

        public bool RemoveStudy(int studyId)
        {
            
            return _studyStorageManager.Remove (studyId);
        }

        public bool UpdateStudy(int studyId, StudyDTO studyDTO)
        {
        
        
            
                var oldStudy = _studyStorageManager.Get(studyId);

                var updatedStudy = ConvertStudy(studyDTO);
            oldStudy.Name = updatedStudy.Name;

            updatedStudy.Items.AddRange(oldStudy.Items) ;
            updatedStudy.ID = oldStudy.ID;
                List<Stage> tempList = new List<Stage>();
            if (oldStudy.Stages.Count != updatedStudy.Stages.Count)
            {
                tempList.AddRange(oldStudy.Stages.ToList().GetRange(0, oldStudy.Stages.Count - 1));
                tempList.AddRange(updatedStudy.Stages.ToList().GetRange(oldStudy.Stages.Count - 1, updatedStudy.Stages.Count - 1));
                oldStudy.Stages = tempList;
            }
                
                _studyStorageManager.Update(oldStudy);
                return true;
            
            
            
        }

        public IEnumerable<Study> SearchStudies(string studyName)
        {
            return (from Study dbStudy in _studyStorageManager.GetAll() where dbStudy.Name.Equals(studyName) select dbStudy).ToList();
        }

        public StudyDTO GetStudy(int studyId)
        {
            return new StudyDTO(_studyStorageManager.Get(studyId));
        }

        public IEnumerable<Study> GetAllStudies()
        {
            return (from Study dbStudy in _studyStorageManager.GetAll() select dbStudy);
        }

        public IEnumerable<TaskRequestDTO> GetTasks(int studyId, int userId, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            var study = _studyStorageManager.GetAll()
                .Where(s => s.ID == studyId)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

            if (study==null)
            {
                throw new NullReferenceException("Study not found");
            }
            if (!study.Team.Users.Select(u => u.ID).Contains(userId))
            {
                throw new ArgumentException("The user is not part of this study");
            }

            var taskIDs = study.CurrentStage().Tasks.Select(t=>t.ID).ToList();
            var visibleFields = study.CurrentStage().VisibleFields;

            return _taskManager.GetTasksDTOs(visibleFields, taskIDs, userId, count, filter, type);
        }

        public IEnumerable<int> GetTasksIDs(int studyId, int userId, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            var study = _studyStorageManager.GetAll()
                .Where(s => s.ID == studyId)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

            if (study == null)
            {
                throw new NullReferenceException("Study not found");
            }
            if (!study.Team.Users.Select(u => u.ID).Contains(userId))
            {
                throw new ArgumentException("The user is not part of this study");
            }

            var taskIDs = study.CurrentStage().Tasks.Select(t => t.ID).ToList();

            return _taskManager.GetTasksIDs(taskIDs, userId, filter, type);
        }

        public TaskRequestDTO GetTask(int taskID)
        {
            return _taskManager.GetTaskDTO(taskID);
        }
    }
}