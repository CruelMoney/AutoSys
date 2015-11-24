using System;
using System.Collections.Generic;
using System.Web.Http;
using Logic.Controllers.Interfaces;
using Logic.Model.DTO;

namespace Logic.Controllers
{
    /// <summary>
    /// Controller to access and modify teams.
    /// </summary>
    internal class TeamController : ApiController, ITeamController
    {
        /// <summary>
        /// Get all teams.
        /// </summary>
        /// <param name="name">Search for teams which match the specified name.</param>
        public IEnumerable<Team> Get(string name = "")
        {
            // GET: api/Team
            // GET: api/Team?name=untouchables
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the team with the specific ID.
        /// </summary>
        /// <param name="id">The ID of the team to retrieve.</param>
        public Team Get(int id)
        {
            // GET: api/Team/5
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new Team.
        /// </summary>
        /// <param name="team">The new team to create.</param>
        public IHttpActionResult Post([FromBody]Team team)
        {
            // POST: api/Team
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update the team with the specified ID.
        /// The list of users part of the team can not be modified once it has been created.
        /// </summary>
        /// <param name="id">The ID of the team to update.</param>
        /// <param name="user">The new team data.</param>
        public IHttpActionResult Put(int id, [FromBody]Team user)
        {
            // PUT: api/Team/5
            throw new NotImplementedException();
        }

        /// <summary>
        /// Delete the team with the specified ID.
        /// A team can not be deleted when it is participating in a study.
        /// </summary>
        /// <param name="id">The ID of the team to delete.</param>
        public IHttpActionResult Delete(int id)
        {
            // DELETE: api/Team/5
            throw new NotImplementedException();
        }
    }
}
