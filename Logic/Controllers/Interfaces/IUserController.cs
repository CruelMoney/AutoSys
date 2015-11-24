using System;
using System.Collections.Generic;
using System.Web.Http;
using Logic.Model.DTO;

namespace Logic.Controllers.Interfaces
{
    /// <summary>
    /// Controller to access and modify users.
    /// </summary>
    public interface IUserController
    {
        /// <summary>
        /// Get all users.
        /// </summary>
        /// <param name="name">Search for users which match the specified name.</param>
        IEnumerable<User> Get(string name = "");

        /// <summary>
        /// Get the user with the specific ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        User Get(int id);

        /// <summary>
        /// Get all study IDs of studies a given user is part of.
        /// </summary>
        /// <param name="id">The ID of the user to get study IDs for.</param>
        /// <returns></returns>
        [Route("{id}/StudyIDs")]
        IEnumerable<int> GetStudyIDs(int id);

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user">The new user to create.</param>
        IHttpActionResult Post([FromBody] User user);

        /// <summary>
        /// Update the user with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="user">The new user data.</param>
        IHttpActionResult Put(int id, [FromBody] User user);

        /// <summary>
        /// Delete the user with the specified ID.
        /// A user can not be deleted when the user is participating in a study.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        IHttpActionResult Delete(int id);
    }
}
