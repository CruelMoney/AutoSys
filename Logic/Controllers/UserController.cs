using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using Logic.Controllers.Interfaces;
using Logic.Model.DTO;
using Logic.TeamCRUD;

namespace Logic.Controllers
{
    /// <summary>
    /// Controller to access and modify users.
    /// </summary>
    [RoutePrefix("api/User")]
    internal class UserController : ApiController, IUserController
    {
        private readonly UserManager _manager = new UserManager();

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="name">Search for users which match the specified name.</param>
        public IEnumerable<User> Get(string name = "")
        {
            // GET: api/User
            // GET: api/User?name=alice
            return _manager.SearchUsers(name);
        }

        /// <summary>
        /// Get the user with the specific ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        public User Get(int id)
        {
            // GET: api/User/5
            return _manager.GetUser(id);
        }

        /// <summary>
        /// Get all study IDs of studies a given user is part of.
        /// </summary>
        /// <param name="id">The ID of the user to get study IDs for.</param>
        /// <returns></returns>
        [Route("{id}/StudyIDs")]
        public IEnumerable<int> GetStudyIDs(int id)
        {
            // GET: api/User/5/StudyIDs
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user">The new user to create.</param>
        public IHttpActionResult Post([FromBody]User user)
        {
            // POST: api/User
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            user.Id = _manager.CreateUser(user);

            return CreatedAtRoute("DefaultApi", new { id = user.Id }, user);
        }

        /// <summary>
        /// Update the user with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="user">The new user data.</param>
        public IHttpActionResult Put(int id, [FromBody]User user)
        {
            // PUT: api/User/5

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Is the user id always entered?
            if (id != user.Id)
            {
                return BadRequest();
            }

            var updated =  _manager.UpdateUser(id, user);

            if (!updated)
            {
                return NotFound();
            }

            return StatusCode(HttpStatusCode.NoContent);           
        }

        /// <summary>
        /// Delete the user with the specified ID.
        /// A user can not be deleted when the user is participating in a study.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
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
