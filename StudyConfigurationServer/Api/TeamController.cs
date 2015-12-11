using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using StudyConfigurationServer.Api.Interfaces;
using StudyConfigurationServer.Logic.TeamCRUD;
using StudyConfigurationServer.Models.DTO;
using System;

namespace StudyConfigurationServer.Api
{
    /// <summary>
    /// Controller to access and modify teams.
    /// </summary>
    public class TeamController : ApiController, ITeamController
    {
        private readonly TeamManager _manager = new TeamManager();
        /// <summary>
        /// Get all teams.
        /// </summary>
        /// <param name="name">Search for teams which match the specified name.</param>
        public IHttpActionResult Get(string name = "")
        {
            // GET: api/Team
            // GET: api/Team?name=untouchables

            try
            {
                var teams = name.Equals(string.Empty) ? _manager.GetAllTeamDTOs() : _manager.SearchTeamDTOs(name);           
                return Ok(teams);
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
           
           
        }

        /// <summary>
        /// Get the TeamDTO with the specific ID.
        /// </summary>
        /// <param name="id">The ID of the TeamDTO to retrieve.</param>
        public IHttpActionResult Get(int id)
        {
            // GET: api/Team/5
            try
            {
                return Ok(_manager.GetTeamDTO(id));
            }
            catch (NullReferenceException)
            {
                return NotFound();
            }

        }

        /// <summary>
        /// Create a new TeamDTO.
        /// </summary>
        /// <param name="teamDto">The new TeamDTO to create.</param>
        public IHttpActionResult Post([FromBody]TeamDTO teamDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
            teamDto.Id = _manager.CreateTeam(teamDto);
                return CreatedAtRoute("DefaultApi", new { id = teamDto.Id }, teamDto);
            }
            catch (NullReferenceException)
            {
                return BadRequest();
            }

        }

        /// <summary>
        /// Update the TeamDTO with the specified ID.
        /// The list of users part of the TeamDTO can not be modified once it has been created.
        /// </summary>
        /// <param name="id">The ID of the TeamDTO to update.</param>
        /// <param name="user">The new TeamDTO data.</param>
        public IHttpActionResult Put(int id, [FromBody]TeamDTO teamDto)
        {
            // PUT: api/Team/5
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var updated = _manager.UpdateTeam(id, teamDto);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete the TeamDTO with the specified ID.
        /// A TeamDTO can not be deleted when it is participating in a study.
        /// </summary>
        /// <param name="id">The ID of the TeamDTO to delete.</param>
        public IHttpActionResult Delete(int id)
        {
            // DELETE: api/Team/5
            try
            {
                var deleted = _manager.RemoveTeam(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
