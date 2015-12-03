﻿using System.Collections.Generic;
using System.Web.Http;
using StudyConfigurationServer.Logic.StudyConfiguration;
using StudyConfigurationServer.Logic.TeamCRUD;
using StudyConfigurationServer.Models;

namespace StudyConfigurationServer.Api
{
    public class StudyConfigurationController : ApiController
    {
        private readonly StudyManager _manager = new StudyManager();
        // GET: api/StudyConfiguration
        public IEnumerable<Study> Get()
        {
            return _manager.GetAllStudies();
        }

        // GET: api/StudyConfiguration/5
        public Study Get(int id)
        {
            return _manager.GetStudy(id);
        }

        // POST: api/StudyConfiguration
        public void Post([FromBody]Study study)
        {
            _manager.CreateStudy(study);
        }

        // PUT: api/StudyConfiguration/5
        public void Put(int id, [FromBody]Study study)
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
