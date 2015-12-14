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
            _taskManager = new TaskManager(repo);
            _studyStorageManager = new StudyStorageManager(repo);
        }

        public StudyManager(StudyStorageManager storageManager, TaskManager taskManager, TeamStorageManager teamStorage)
        {
            _studyStorageManager = storageManager;
            _taskManager = taskManager;
            _teamStorage = teamStorage;
        }

        public StudyManager(EntityFrameworkGenericRepository<StudyContext> repo)
        {
            _studyStorageManager = new StudyStorageManager(repo);
            _taskManager = new TaskManager(repo);
            _teamStorage = new TeamStorageManager(repo);
        }

        //TODO check if whole study finished
        public bool DeliverTask(int studyID, int taskID, TaskSubmissionDTO taskDTO)
        {
           
            bool deliverSucces ;

            try
            {
               deliverSucces =  _taskManager.DeliverTask(taskID, taskDTO);
            }
            catch (Exception e)
            {
               
               throw;
            }
            
             var currentStudy = _studyStorageManager.GetAllStudies()
                .Where(s => s.ID == studyID)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

             
            //Determine if the stage is finished
            if (currentStudy.CurrentStage().Tasks.Select(t=>t.ID).ToList().TrueForAll(t=>_taskManager.TaskIsFinished(t)))
            {
               MoveToNextPhase(currentStudy);
            }

            return deliverSucces;
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
        }

        private void FinishReviewPhase(Study study)
        {
            var currentStage = study.CurrentStage();
            var taskIDs = currentStage.Tasks.Select(t => t.ID).ToList();
            var criteria = currentStage.Criteria;
            
            //Generate the validation tasks and get id's on the items to be excluded
            var excludedItemIDs = _taskManager.GetExcludedItems(
                taskIDs, criteria).ToList();

            //Remove the excluded items from the study
            study.Items.RemoveAll(i => excludedItemIDs.Contains(i.ID));

            //Generate the validation tasks
            var validationTasks = StartValidationPhase(study).ToList();

            if (!validationTasks.Any())
            {
                //Start the next review phase if no validation tasks
                StartReviewPhase(study);
            }
            else
            {
                StartValidationPhase(study);
            }

            _studyStorageManager.UpdateStudy(study);
        }

        private void FinishConflictPhase(Study study)
        {
            var currentStage = study.CurrentStage();
            var taskIDs = currentStage.Tasks.Select(t => t.ID).ToList();
            var criteria = currentStage.Criteria;

            var excludedItems = _taskManager.GetExcludedItems(taskIDs, criteria);

            //Remove the excluded items from the study, and move to next stage
            study.Items.RemoveAll(i => excludedItems.Contains(i.ID));

            StartReviewPhase(study);

            _studyStorageManager.UpdateStudy(study);
        }

        private IEnumerable<StudyTask> StartReviewPhase(Study study)
        {
            var stage = study.CurrentStage();
            var reviewers =
                stage.Users.Where(u => u.StudyRole == UserStudies.Role.Reviewer).Select(u => u.User).ToList();
           var reviewTasks =
                _taskManager.GenerateReviewTasks(study.Items, reviewers, stage.Criteria,
                    stage.DistributionRule).ToList();
            stage.Tasks = reviewTasks;
            stage.CurrentTaskType = StudyTask.Type.Review;
            
            return reviewTasks;
        }

        private IEnumerable<StudyTask> StartValidationPhase(Study study)
        {
            var stage = study.CurrentStage();

            var taskIDs = stage.Tasks.Where(t => t.ContainsConflictingData()).Select(t=>t.ID).ToList();
            var criteria = stage.Criteria;
            var validators = stage.Users.Where(u => u.StudyRole == UserStudies.Role.Validator).Select(u => u.User).ToList();

            //Update stage to start the validations and generate the validation tasks
            stage.CurrentTaskType = StudyTask.Type.Conflict;
            var validationTasks = _taskManager.GenerateValidationTasks(taskIDs, criteria, validators, stage.DistributionRule);
            stage.Tasks.AddRange(validationTasks);
            
            return validationTasks;
        }


        public int CreateStudy(StudyDTO studyDTO)
        {

            var study = ConvertStudy(studyDTO);
            _studyStorageManager.SaveStudy(study);

            return study.ID;
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


                stage.Criteria.Add(new Criteria()
                {
                    Name = stageDto.Criteria.Name,
                    DataMatch = stageDto.Criteria.DataMatch,
                    DataType = (DataField.DataType)Enum.Parse(typeof(DataField.DataType), stageDto.Criteria.DataType.ToString()),
                    Description = stageDto.Criteria.Description,
                    Rule = (Criteria.CriteriaRule)Enum.Parse(typeof(Criteria.CriteriaRule), stageDto.Criteria.Rule.ToString()),
                    TypeInfo = stageDto.Criteria.TypeInfo
                });


                study.Stages.Add(stage);

                if (firstStage)
                {
                    stage.IsCurrentStage = true;
                    StartReviewPhase(study);
                }

                firstStage = false;

            }
            return study;
        }

        public bool RemoveStudy(int studyId)
        {
            return _studyStorageManager.RemoveStudy(studyId);
        }

        /*public bool UpdateStudy(int studyId, StudyDTO study)
        {
            var savedStudy = _studyStorageManager.GetStudy(studyId);
            var updatedStudy = ConvertStudy(study);


        }*/

        public IEnumerable<Study> SearchStudies(string studyName)
        {
            return (from Study dbStudy in _studyStorageManager.GetAllStudies() where dbStudy.Name.Equals(studyName) select dbStudy).ToList();
        }

        public StudyDTO GetStudy(int studyId)
        {
            return new StudyDTO(_studyStorageManager.GetStudy(studyId));
        }

        public IEnumerable<Study> GetAllStudies()
        {
            return (from Study dbStudy in _studyStorageManager.GetAllStudies() select dbStudy);
        }

        public IEnumerable<TaskRequestDTO> GetTasks(int studyId, int userId, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            var study = _studyStorageManager.GetAllStudies()
                .Where(s => s.ID == studyId)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

            if (study==null)
            {
                throw new NullReferenceException("Study not found");
            }

            var taskIDs = study.CurrentStage().Tasks.Select(t=>t.ID).ToList();
            var visibleFields = study.CurrentStage().VisibleFields;

            return _taskManager.GetTasksDTOs(visibleFields, taskIDs, userId, count, filter, type);
        }

        public IEnumerable<int> GetTasksIDs(int studyId, int userId, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            var study = _studyStorageManager.GetAllStudies()
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