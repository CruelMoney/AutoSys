using StudyConfigurationServer.Logic.StorageManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Storage.Repository;
using StudyConfigurationServer.Logic.TaskManagement;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Logic.StudyConfiguration
{
    public class StudyManager
    {
        private readonly StudyStorageManager _studyStorageManager;
        private readonly TaskController _taskController;

        public StudyManager()
        {
           _taskController = new TaskController();

           

            _studyStorageManager = new StudyStorageManager();
        }

        public StudyManager(StudyStorageManager storageManager, TaskController taskController)
        {
            _studyStorageManager = storageManager;
            _taskController = taskController;
        }

        public bool DeliverTask(int studyID, int taskID, TaskSubmissionDTO taskDTO)
        {

            var currentStudy = _studyStorageManager.GetStudy(studyID);
            bool deliverSucces;

            try
            {
               deliverSucces =  _taskController.DeliverTask(taskID, taskDTO);
            }
            catch (Exception)
            {
                throw;
            }
            
            //Determine if the stage is finished
            if (_taskController.StageIsFinished(currentStudy.CurrentStage()))
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

            var excludedItemIDs = _taskController.GenerateValidationTasks(currentStage.TaskIDs, criteria, validators, currentStage.DistributionRule);

            //Update stage to start the validations
            currentStage.CurrentTaskType = StudyTask.Type.Conflict;

            //Remove the excluded items from the study
            var currentStudy = _studyStorageManager.GetStudy(currentStage.StudyID);
            currentStudy.Items.RemoveAll(i => excludedItemIDs.Contains(i.Id));

            //save the study with the updated items and updated stage
            _studyStorageManager.UpdateStudy(currentStudy);
        }

        private void FinishConflictPhase(Stage currentStage)
        {
            var excludedItems = _taskController.CriteriaValidateTasks(currentStage.Criteria, currentStage.TaskIDs);

            //Remove the excluded items from the study, and move to next stage
            var currentStudy = _studyStorageManager.GetStudy(currentStage.StudyID);
            currentStudy.Items.RemoveAll(i => excludedItems.Contains(i.Id));
            currentStudy.MoveToNextStage();

            //save the study with the updated items and updated stage
            _studyStorageManager.UpdateStudy(currentStudy);

            //Find the users that are reviewers for this stage.
            var reviewers = currentStage.Users.Where(u => u.StudyRole == UserStudies.Role.Reviewer).Select(u => u.User).ToList();

            //Generate the new review tasks
            _taskController.GenerateReviewTasks(currentStudy.Items, reviewers, currentStudy.CurrentStage().DistributionRule, currentStudy.CurrentStage().Criteria);
        }

        public int CreateStudy(Study study)
        {
            return _studyStorageManager.SaveStudy(study);
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
            var study = _studyStorageManager.GetStudy(studyId);
            var logicType = (StudyTask.Type) Enum.Parse(typeof (StudyTask.Type), type.ToString());
            return _taskController.GetTasksForUser(study, userId, count, filter, logicType);
        }
    }
}