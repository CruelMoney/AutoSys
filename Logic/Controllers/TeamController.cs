using System;
using System.Collections.Generic;
using System.Web.Http;
using Logic.Controllers.Interfaces;
using Logic.Model.DTO;
using Logic.StorageManagement;
using Logic.TeamCRUD;
using System.Net;

namespace Logic.Controllers
{
    /// <summary>
    /// Controller to access and modify teams.
    /// </summary>
    internal class TeamController : ApiController, ITeamController
    {
        private readonly TeamManager _manager = new TeamManager();
        /// <summary>
        /// Get all teams.
        /// </summary>
        /// <param name="name">Search for teams which match the specified name.</param>
        public IEnumerable<Team> Get(string name = "")
        {
            // GET: api/Team
            // GET: api/Team?name=untouchables

            
            return _manager.SearchTeams(name);
        }

        /// <summary>
        /// Get the team with the specific ID.
        /// </summary>
        /// <param name="id">The ID of the team to retrieve.</param>
        public Team Get(int id)
        {

            _manager.GetTeam(id);

            // GET: api/Team/5
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new Team.
        /// </summary>
        /// <param name="team">The new team to create.</param>
        public IHttpActionResult Post([FromBody]Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            team.Id = _manager.CreateTeam(team);

            return CreatedAtRoute("DefaultApi", new { id = team.Id }, team);
        }

        /// <summary>
        /// Update the team with the specified ID.
        /// The list of users part of the team can not be modified once it has been created.
        /// </summary>
        /// <param name="id">The ID of the team to update.</param>
        /// <param name="user">The new team data.</param>
        public IHttpActionResult Put(int id, [FromBody]Team team)
        {
            // PUT: api/Team/5
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != team.Id)
            {
                return BadRequest();
            }

            var updated = _manager.UpdateTeam(id, team);

            if (!updated)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Delete the team with the specified ID.
        /// A team can not be deleted when it is participating in a study.
        /// </summary>
        /// <param name="id">The ID of the team to delete.</param>
        public IHttpActionResult Delete(int id)
        {
            // DELETE: api/Team/5
            var deleted = _manager.RemoveTeam(id);
            if (!deleted)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
