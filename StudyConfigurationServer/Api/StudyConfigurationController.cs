#region Using

using System;
using System.Collections.Generic;
using System.Web.Http;
using StudyConfigurationServer.Logic.StudyConfiguration;
using StudyConfigurationServer.Models.DTO;

#endregion

namespace StudyConfigurationServer.Api
{
    public class StudyConfigurationController : ApiController
    {
        private readonly StudyManager _manager = new StudyManager();

        /// <summary>
        ///     Get all teams.
        /// </summary>
        /// <param name="name">Search for teams which match the specified name.</param>
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

        // GET: api/StudyConfiguration
        public IEnumerable<StudyDto> Get()
        {
            return _manager.GetAllStudies();
        }

        // GET: api/StudyConfiguration/5
        public StudyDto Get(int id)
        {
            return _manager.GetStudy(id);
        }

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

        // PUT: api/StudyConfiguration/5
        public void Put(int id, [FromBody] StudyDto study)
        {
            _manager.UpdateStudy(id, study);
        }


        // DELETE: api/StudyConfiguration/5
        public void Delete(int id)
        {
            _manager.RemoveStudy(id);
        }
    }
}