using StudyConfigurationServer.Logic.StorageManagement;
using System;
using System.Collections.Generic;
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

namespace StudyConfigurationServer.Logic.StudyConfiguration
{
    public class StudyManager
    {
        private readonly StudyStorageManager _studyStorageManager;
        private readonly TeamStorageManager _teamStorageManager;
        private readonly TaskManager _taskManager;
        

        public StudyManager()
        {
           _taskManager = new TaskManager();
            _studyStorageManager = new StudyStorageManager();
        }

        public StudyManager(StudyStorageManager storageManager, TaskManager taskManager)
        {
            _studyStorageManager = storageManager;
            _taskManager = taskManager;
        }
        
        //TODO check if whole study finished
        public bool DeliverTask(int studyID, int taskID, TaskSubmissionDTO taskDTO)
        {

            var currentStudy = _studyStorageManager.GetStudy(studyID);
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
            if (_taskManager.StageIsFinished(currentStudy.CurrentStage()))
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
            var excludedItemIDs = _taskManager.GenerateValidationTasks(currentStage.TaskIDs, criteria, validators, currentStage.DistributionRule);

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
            var excludedItems = _taskManager.CriteriaValidateTasks(currentStage.Criteria, currentStage.TaskIDs);

            //Remove the excluded items from the study, and move to next stage
            var currentStudy = _studyStorageManager.GetStudy(currentStage.StudyID);
            currentStudy.Items.RemoveAll(i => excludedItems.Contains(i.Id));
            currentStudy.MoveToNextStage();

            //save the study with the updated items and updated stage
            _studyStorageManager.UpdateStudy(currentStudy);

            //Find the users that are reviewers for this stage.
            var reviewers = currentStage.Users.Where(u => u.StudyRole == UserStudies.Role.Reviewer).Select(u => u.User).ToList();

            //Generate the new review tasks
            _taskManager.GenerateReviewTasks(currentStudy.Items, reviewers, currentStudy.CurrentStage().DistributionRule, currentStudy.CurrentStage().Criteria);
        }


        public int CreateStudy(StudyDTO studyDTO)
        {
            var study = new Study()
            {
                IsFinished = false,
                Name = studyDTO.Name,
                Team = _teamStorageManager.GetTeam(studyDTO.TeamID),
                Items = new List<Item>(),
                Stages = new List<Stage>()
            };

            //Parse items
            var parser = new BibTexParser(new ItemValidator());
            var fileString = System.Text.Encoding.Default.GetString(studyDTO.Items);
            study.Items = parser.Parse(fileString);

            foreach (var stageDto in studyDTO.Stages)
            {
                var stage = new Stage()
                {
                    Criteria = stageDto.Criteria,
                    CurrentTaskType = StudyTask.Type.Review,
                    DistributionRule = stageDto.DistributionRule,
                    VisibleFields = stageDto.VisibleFields,
                    Users = new List<UserStudies>(),
                    Name = stageDto.Name
                };

                foreach (var reviewer in stageDto.ReviewerIDs)
                {
                    stage.Users.Add(new UserStudies()
                    {
                        StudyRole = UserStudies.Role.Reviewer,
                        User = _teamStorageManager.GetUser(reviewer)
                    });
                }

                foreach (var validator in stageDto.ValidatorIDs)
                {
                    stage.Users.Add(new UserStudies()
                    {
                        StudyRole = UserStudies.Role.Validator,
                        User = _teamStorageManager.GetUser(validator)
                    });
                }

                study.Stages.Add(stage);
            }

            //TODO This will not work, the stage havent got an id yet
            study.CurrentStageID = study.Stages.First().Id;

            var studyID = _studyStorageManager.SaveStudy(study);

           

            //Find the users that are reviewers for this stage.
            var reviewers = study.CurrentStage().Users.Where(u => u.StudyRole == UserStudies.Role.Reviewer).Select(u => u.User).ToList();

            //Generate the new review tasks
            _taskManager.GenerateReviewTasks(study.Items, reviewers, study.CurrentStage().DistributionRule, study.CurrentStage().Criteria);

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
            return (from Study dbStudy in _studyStorageManager.GetAllStudies() select dbStudy).ToList();
        }

        public IEnumerable<TaskRequestDTO> getTasks(int studyId, int userId, int count, TaskRequestDTO.Filter filter, TaskRequestDTO.Type type)
        {
            throw new NotImplementedException();
            var study = _studyStorageManager.GetStudy(studyId);
            var taskType = (StudyTask.Type)Enum.Parse(typeof(StudyTask.Type), type.ToString());
           // return _taskManager.GetTasksForUser(study, userId, count, filter, taskType);
        }
    }
}