using System;
using System.Collections.Generic;
using System.Web.Http;
using StudyConfigurationServer.Logic.StorageManagement;
using StudyConfigurationServer.Logic.StudyConfiguration;
using StudyConfigurationServer.Logic.TeamCRUD;
using StudyConfigurationServer.Models;
using StudyConfigurationServer.Models.DTO;

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
        public IHttpActionResult Post([FromBody]StudyDTO study)
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
        public void Put(int id, [FromBody]StudyDTO study)
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
