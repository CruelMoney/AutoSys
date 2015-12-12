using StudyConfigurationServer.Logic.StorageManagement;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            var currentStudy = _studyStorageManager.GetAllStudies()
                .Where(s => s.Id == studyID)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

            bool deliverSucces;

            try
            {
               deliverSucces =  _taskManager.DeliverTask(taskID, taskDTO);
            }
            catch (Exception)
            {
                throw;
            }
            
            //Determine if the stage is finished
            if (currentStudy.CurrentStage().Tasks.Select(t=>t.Id).ToList().TrueForAll(t=>_taskManager.TaskIsFinished(t)))
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
                    FinishReviewPhase(currentStage);
                    break;
                case StudyTask.Type.Conflict:
                    FinishConflictPhase(currentStage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void FinishReviewPhase(Stage currentStage)
        {
            var criteria = currentStage.Criteria;

            var validators = currentStage.Users.Where(u => u.StudyRole == UserStudies.Role.Validator).Select(u => u.User).ToList();

            //Generate the validation tasks and get id's on the items to be excluded
            var excludedItemIDs = _taskManager.GenerateValidationTasks(
                currentStage.Tasks.Select(t=>t.Id).ToList(), 
                criteria, 
                validators, 
                currentStage.DistributionRule);

            //Remove the excluded items from the study
            var currentStudy = _studyStorageManager.GetStudy(currentStage.StudyID);
            currentStudy.Items.RemoveAll(i => excludedItemIDs.Contains(i.Id));

            //Update stage to start the validations
            currentStage.CurrentTaskType = StudyTask.Type.Conflict;

            //save the study with the updated items and updated stage
            _studyStorageManager.UpdateStudy(currentStudy);
        }

        private void FinishConflictPhase(Stage currentStage)
        {
            var excludedItems = _taskManager.CriteriaValidateTasks(currentStage.Criteria, currentStage.Tasks.Select(t=>t.Id).ToList());

            //Remove the excluded items from the study, and move to next stage
            var currentStudy = _studyStorageManager.GetStudy(currentStage.StudyID);
            currentStudy.Items.RemoveAll(i => excludedItems.Contains(i.Id));
            currentStudy.MoveToNextStage();

            //save the study with the updated items and updated stage
            _studyStorageManager.UpdateStudy(currentStudy);

            //Find the users that are reviewers for this stage.
            var reviewers = currentStage.Users.Where(u => u.StudyRole == UserStudies.Role.Reviewer).Select(u => u.User).ToList();

            //Generate the new review tasks
            _taskManager.GenerateReviewTasks(currentStudy.Items, reviewers, currentStage.Criteria, currentStage.DistributionRule);
        }


        public int CreateStudy(StudyDTO studyDTO)
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
                    DistributionRule = (Stage.Distribution) Enum.Parse(typeof(Stage.Distribution), stageDto.DistributionRule.ToString()),
                    VisibleFields = new List<FieldType>(),
                    Users = new List<UserStudies>(),
                    Criteria = new List<Criteria>(),
                    
                };

                stageDto.VisibleFields.ForEach(
                    f => stage.VisibleFields.Add(new FieldType (f.ToString())));

                stageDto.ReviewerIDs.ForEach(u=>
                    stage.Users.Add(new UserStudies()
                    {
                        StudyRole = UserStudies.Role.Reviewer,
                        User = _teamStorage.GetUser(u)
                    }));
               
                stageDto.ValidatorIDs.ForEach(u=>
                    stage.Users.Add(new UserStudies()
                    {
                        StudyRole = UserStudies.Role.Validator,
                        User = _teamStorage.GetUser(u)
                    }));
                
                stageDto.Criteria.ForEach(
                    c=> stage.Criteria.Add(new Criteria()
                {
                    Name = c.Name,
                    DataMatch = c.DataMatch,
                    DataType = (DataField.DataType) Enum.Parse(typeof(DataField.DataType), c.DataType.ToString()),
                    Description = c.Description,
                    Rule = (Criteria.CriteriaRule) Enum.Parse(typeof(Criteria.CriteriaRule), c.Rule.ToString()),
                    TypeInfo = c.TypeInfo
                    }));

                if (firstStage)
                {
                    //Find the users that are reviewers for this stage.
                    var reviewers = stage.Users.Where(u => u.StudyRole == UserStudies.Role.Reviewer).Select(u => u.User).ToList();

                    stage.Tasks = _taskManager.GenerateReviewTasks(study.Items, reviewers, stage.Criteria, stage.DistributionRule).ToList();
                }

                firstStage = false;

                study.Stages.Add(stage);
            }
            
            var studyID = _studyStorageManager.SaveStudy(study);

            //Move to next stage to get the right currentStageID
            study.MoveToNextStage();
            
            _studyStorageManager.UpdateStudy(study);
        
            return studyID;
        }

        public bool RemoveStudy(int studyId)
        {
            return _studyStorageManager.RemoveStudy(studyId);
        }

        public bool UpdateStudy(int studyId, Study study)
        {
            return _studyStorageManager.UpdateStudy(study);
        }

        public IEnumerable<Study> SearchStudies(string studyName)
        {
            return (from Study dbStudy in _studyStorageManager.GetAllStudies() where dbStudy.Name.Equals(studyName) select dbStudy).ToList();
        }

        public Study GetStudy(int studyId)
        {
            return _studyStorageManager.GetStudy(studyId);
        }

        public IEnumerable<Study> GetAllStudies()
        {
            return (from Study dbStudy in _studyStorageManager.GetAllStudies() select dbStudy);
        }

        public IEnumerable<TaskRequestDTO> getTasks(int studyId, int userId, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            var study = _studyStorageManager.GetAllStudies()
                .Where(s => s.Id == studyId)
                .Include(s => s.Stages.Select(t => t.Tasks))
                .FirstOrDefault();

            var taskIDs = study.CurrentStage().Tasks.Select(t=>t.Id).ToList();
            var visibleFields = study.CurrentStage().VisibleFields;

            return _taskManager.GetTasksDTOs(visibleFields, taskIDs, userId, count, filter, type);
        }
    }
}