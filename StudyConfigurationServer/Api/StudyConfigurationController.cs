﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace StudyConfigurationServer.Controllers
{
    public class StudyConfigurationController : ApiController
    {
        // GET: api/StudyConfiguration
        public IEnumerable<string> Get()
        {
            return new string[] { "WOLOLOLO", "WOLOLOLO" };
        }

        // GET: api/StudyConfiguration/5
        public string Get(int id)
        {
            return "Det virker";
        }

        // POST: api/StudyConfiguration
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/StudyConfiguration/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/StudyConfiguration/5
        public void Delete(int id)
        {
        }
    }
}
