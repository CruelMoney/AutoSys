#region Using

using System.Linq;
using Storage.Repository;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.Data;

#endregion

namespace StudyConfigurationServer.Logic.StorageManagement
{
    /// <summary>
    /// StorageManager responsible for storing, retrieving, updating and deleting studies
    /// </summary>
    public class StudyStorageManager : IStudyStorageManager
    {
        private readonly IGenericRepository _studyRepo;


        public StudyStorageManager()
        {
            _studyRepo = new EntityFrameworkGenericRepository<StudyContext>();
        }

        public StudyStorageManager(IGenericRepository repo)
        {
            _studyRepo = repo;
        }

        /// <summary>
        /// Store a study in the repository
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        public int Save(Study study)
        {
            var returnValue = _studyRepo.Create(study);


            return returnValue;
        }

        /// <summary>
        /// Delete a study
        /// </summary>
        /// <param name="studyWithIdToDelete"></param>
        /// <returns></returns>
        public bool Remove(int studyWithIdToDelete)
        {
            return _studyRepo.Delete(_studyRepo.Read<Study>(studyWithIdToDelete));
        }

        /// <summary>
        /// Update a study
        /// </summary>
        /// <param name="study"></param>
        /// <returns></returns>
        public bool Update(Study study)
        {
            var returnValue = _studyRepo.Update(study);


            return returnValue;
        }

        /// <summary>
        /// Retrieve all studies
        /// </summary>
        /// <returns></returns>
        public IQueryable<Study> GetAll()
        {
            return _studyRepo.Read<Study>();
        }

        /// <summary>
        /// Retrieve a single study
        /// </summary>
        /// <param name="studyId"></param>
        /// <returns></returns>
        public Study Get(int studyId)
        {
            return _studyRepo.Read<Study>(studyId);
        }
    }
}