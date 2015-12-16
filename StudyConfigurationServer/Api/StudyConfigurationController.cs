#region Using

using System;
using System.Collections.Generic;
using System.Web.Http;
using StudyConfigurationServer.Logic.StudyManagement;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServer.Api
{
    public class StudyConfigurationController : ApiController
    {
        private readonly IStudyManager _manager = new StudyManager();

        /// <summary>
        ///     Search for study with the name. If no name given all studies are returned. 
        /// </summary>
        /// <param name="name">Search for studies which match the specified name.</param>
        public IEnumerable<StudyDto> Get(string name = "")
        {
            // GET: api/Team
            // GET: api/Team?name=untouchables

            try
            {
                var studies = name.Equals(string.Empty) ? _manager.GetAllStudies() : _manager.SearchStudies(name);
                return studies;
            }
            catch (NullReferenceException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get all studies.
        /// </summary>
        /// <returns></returns>
        // GET: api/StudyConfiguration
        public IEnumerable<StudyDto> Get()
        {
            return _manager.GetAllStudies();
        }

        /// <summary>
        /// Get study with the given id
        /// </summary>
        /// <param name="id">Id of the study</param>
        /// <returns></returns>
        // GET: api/StudyConfiguration/5
        public StudyDto Get(int id)
        {
            return _manager.GetStudy(id);
         
        }

        /// <summary>
        /// Create a new study in the system. 
        /// </summary>
        /// <param name="study"> A studyDTO json serialized</param>
        /// <returns></returns>
        // POST: api/StudyConfiguration
        public IHttpActionResult Post([FromBody] StudyDto study)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(_manager.CreateStudy(study));
            }
            catch (NullReferenceException)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Update a existing study. Not supporting updating existing stages, teams or items. You can add new stages. 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="study"></param>
        // PUT: api/StudyConfiguration/5
        public void Put(int id, [FromBody] StudyDto study)
        {
            _manager.UpdateStudy(id, study);
        }

        /// <summary>
        /// Delete existing study.
        /// </summary>
        /// <param name="id">ID of the study</param>
        // DELETE: api/StudyConfiguration/5
        public void Delete(int id)
        {
            _manager.RemoveStudy(id);
        }
    }
}