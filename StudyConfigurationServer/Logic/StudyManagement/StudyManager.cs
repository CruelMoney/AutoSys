#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Ajax.Utilities;
using Storage.Repository;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.StudyExecution;
using StudyConfigurationServer.Logic.StudyManagement.BiblographyParser;
using StudyConfigurationServer.Logic.StudyManagement.BiblographyParser.bibTex;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;
using StudyConfigurationServer.Models.DTO;
using FieldType = StudyConfigurationServer.Models.FieldType;

#endregion

namespace StudyConfigurationServer.Logic.StudyManagement
{
    /// <summary>
    /// A A manager responsible for study logic regarding create, retrieve, delete and update
    /// </summary>
    public class StudyManager : IStudyManager
    {
        private readonly IStudyStorageManager _studyStorageManager;
        private readonly ITeamStorageManager _teamStorage;
        private readonly IStudyExecutionController _studyExecutionController;
        private readonly StudyOverview _studyOverview;

        public StudyManager()
        {
            var repo = new EntityFrameworkGenericRepository<StudyContext>();
            _teamStorage = new TeamStorageManager(repo);
            _studyStorageManager = new StudyStorageManager(repo);
            _studyExecutionController = new StudyExecutionController();
            _studyOverview = new StudyOverview();
        }

        public StudyManager(IStudyStorageManager storageManager, ITeamStorageManager teamStorage, IStudyExecutionController studyExecutionController, StudyOverview studyOverview)
        {
            _studyStorageManager = storageManager;
            _teamStorage = teamStorage;
            _studyExecutionController = studyExecutionController;
            _studyOverview = studyOverview;
        }
        
        /// <summary>
        /// Convert a studyDTO to a study object
        /// </summary>
        /// <param name="studyDto">DTO to be converted</param>
        /// <returns></returns>
        private Study ConvertStudy(StudyDto studyDto)
        {
            var study = new Study
            {
                IsFinished = false,
                Name = studyDto.Name,
                Team = _teamStorage.GetTeam(studyDto.Team.Id),
                Items = new List<Item>(),
                Stages = new List<Stage>()
            };
            //Parse items
            var parser = new BibTexParser(new ItemValidator());
            var fileString = Encoding.Default.GetString(studyDto.Items);
            study.Items = parser.Parse(fileString);


            var firstStage = true;

            foreach (var stageDto in studyDto.Stages)
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

        /// <summary>
        /// Create a study and store it in the repository
        /// </summary>
        /// <param name="studyDto"></param>
        /// <returns></returns>
        public int CreateStudy(StudyDto studyDto)
        {
            var study = ConvertStudy(studyDto);
            _studyExecutionController.ExecuteStudy(study);

            _studyStorageManager.Save(study);

            return study.ID;
        }

        /// <summary>
        /// Create a stage for a study
        /// </summary>
        /// <param name="stageDto">The stageDTO to be created</param>
        /// <returns></returns>
        private Stage CreateStage(StageDto stageDto)
        {
            var stage = new Stage
            {
                Name = stageDto.Name,
                CurrentTaskType = StudyTask.Type.Review,
                DistributionRule =
                    (Stage.Distribution) Enum.Parse(typeof (Stage.Distribution), stageDto.DistributionRule.ToString()),
                VisibleFields = new List<FieldType>(),
                Users = new List<UserStudies>(),
                Criteria = new List<Criteria>()
            };

            stageDto.VisibleFields.ForEach(
                f => stage.VisibleFields.Add(new FieldType(f.ToString())));

            stageDto.ReviewerIDs.ForEach(u =>
                stage.Users.Add(new UserStudies
                {
                    StudyRole = UserStudies.Role.Reviewer,
                    User = _teamStorage.GetUser(u)
                }));

            stageDto.ValidatorIDs.ForEach(u =>
                stage.Users.Add(new UserStudies
                {
                    StudyRole = UserStudies.Role.Validator,
                    User = _teamStorage.GetUser(u)
                }));

            var criteria = new Criteria
            {
                Name = stageDto.Criteria.Name,
                DataMatch = stageDto.Criteria.DataMatch.Select(s => new StoredString {Value = s}).ToArray(),
                DataType =
                    (DataField.DataType) Enum.Parse(typeof (DataField.DataType), stageDto.Criteria.DataType.ToString()),
                Description = stageDto.Criteria.Description,
                Rule =
                    (Criteria.CriteriaRule)
                        Enum.Parse(typeof (Criteria.CriteriaRule), stageDto.Criteria.Rule.ToString())
            };

            if (stageDto.Criteria.TypeInfo != null)
            {
                criteria.TypeInfo = stageDto.Criteria.TypeInfo.Select(s => new StoredString {Value = s}).ToArray();
            }

            stage.Criteria.Add(criteria);

            return stage;
        }

        /// <summary>
        /// Remove a study with a given Id
        /// </summary>
        /// <param name="studyId">Id of study to be deleted</param>
        /// <returns></returns>
        public bool RemoveStudy(int studyId)
        {
            return _studyStorageManager.Remove(studyId);
        }

        public bool UpdateStudy(int studyId, StudyDto studyDto)
        {
            var oldStudy = _studyStorageManager.Get(studyId);

            var updatedStudy = ConvertStudy(studyDto);
            oldStudy.Name = updatedStudy.Name;

            updatedStudy.Items.AddRange(oldStudy.Items);
            updatedStudy.ID = oldStudy.ID;
            List<Stage> tempList = new List<Stage>();
            if (oldStudy.Stages.Count != updatedStudy.Stages.Count)
            {
                tempList.AddRange(oldStudy.Stages.ToList().GetRange(0, oldStudy.Stages.Count - 1));
                tempList.AddRange(updatedStudy.Stages.ToList()
                    .GetRange(oldStudy.Stages.Count - 1, updatedStudy.Stages.Count - 1));
                oldStudy.Stages = tempList;
            }

            _studyStorageManager.Update(oldStudy);
            return true;
        }

        /// <summary>
        /// Search for one or more studies with a given name
        /// </summary>
        /// <param name="studyName">String to search for</param>
        /// <returns></returns>
        public IEnumerable<StudyDto> SearchStudies(string studyName)
        {
            return from Study dbStudy in _studyStorageManager.GetAll()
                where dbStudy.Name.Equals(studyName)
                select new StudyDto(dbStudy);
        }

        /// <summary>
        /// Retrieve a single study with a given studyId
        /// </summary>
        /// <param name="studyId">Id of study to retrieve</param>
        /// <returns></returns>
        public StudyDto GetStudy(int studyId)
        {
            return new StudyDto(_studyStorageManager.Get(studyId));
        }

        /// <summary>
        /// Retrieve all studies in the repository
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StudyDto> GetAllStudies()
        {
            return from Study dbStudy in _studyStorageManager.GetAll()
                select new StudyDto(dbStudy);
        }

        /// <summary>
        /// Get a studyOverview
        /// </summary>
        /// <param name="id">Id of the study to get an overview of</param>
        /// <returns></returns>
        public StudyOverviewDto GetStudyOverview(int id)
        {
            return _studyOverview.GetOverview(id);
        } 

    }
}