using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using StudyConfigurationServer.Api.Interfaces;
using StudyConfigurationServer.Logic.TeamCRUD;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Api
{

    /// <summary>
    /// Controller to access and modify users.
    /// </summary>
    //TODO should not be public, how to test? whaat?
    [RoutePrefix("api/UserDTO")]
    public class UserController : ApiController, IUserController
    {
        private readonly UserManager _manager = new UserManager();

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="name">Search for users which match the specified name.</param>
        public IHttpActionResult Get(string name = "")
        {
            // GET: api/User
            // GET: api/User?name=alice
            IEnumerable<UserDTO> users;

            users = name.Equals(string.Empty) ? _manager.GetAllUsers() : _manager.SearchUsers(name);

            return Ok(users);
        }

        /// <summary>
        /// Get the UserDTO with the specific ID.
        /// </summary>
        /// <param name="id">The ID of the UserDTO to retrieve.</param>
        public IHttpActionResult Get(int id)
        {
            // GET: api/User/5
            return Ok(_manager.GetUser(id));
        }

        /// <summary>
        /// Get all study IDs of studies a given UserDTO is part of.
        /// </summary>
        /// <param name="id">The ID of the UserDTO to get study IDs for.</param>
        /// <returns></returns>
        [Route("{id}/StudyIDs")]
        public IHttpActionResult GetStudyIDs(int id)
        {
            // GET: api/UserDTO/5/StudyIDs
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new UserDTO.
        /// </summary>
        /// <param name="userDto">The new UserDTO to create.</param>
        public IHttpActionResult Post([FromBody]UserDTO userDto)
        {
            
            // POST: api/User
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userDto.Id = _manager.CreateUser(userDto);
            
            return CreatedAtRoute("DefaultApi", new { id = userDto.Id }, userDto);
        }

        /// <summary>
        /// Update the UserDTO with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the UserDTO to update.</param>
        /// <param name="userDto">The new UserDTO data.</param>
        public IHttpActionResult Put(int id, [FromBody]UserDTO userDto)
        {
            // PUT: api/User/5

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Is the UserDTO id always entered?
           /* if (id != userDto.Id)
            {
                return BadRequest();
            }*/

            var updated =  _manager.UpdateUser(id, userDto);

            if (!updated)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);           
        }

        /// <summary>
        /// Delete the UserDTO with the specified ID.
        /// A UserDTO can not be deleted when the UserDTO is participating in a study.
        /// </summary>
        /// <param name="id">The ID of the UserDTO to delete.</param>
        public IHttpActionResult Delete(int id)
        {
            // DELETE: api/User/5
            var deleted = _manager.RemoveUser(id);

            if (!deleted)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}
