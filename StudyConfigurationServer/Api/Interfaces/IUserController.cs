using System.Web.Http;
using StudyConfigurationServer.Models.DTO;

namespace StudyConfigurationServer.Api.Interfaces
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
        IHttpActionResult Get(string name = "");

        /// <summary>
        /// Get the UserDTO with the specific ID.
        /// </summary>
        /// <param name="id">The ID of the UserDTO to retrieve.</param>
        IHttpActionResult Get(int id);

        /// <summary>
        /// Get all study IDs of studies a given UserDTO is part of.
        /// </summary>
        /// <param name="id">The ID of the UserDTO to get study IDs for.</param>
        /// <returns></returns>
        [Route("{id}/StudyIDs")]
        IHttpActionResult GetStudyIDs(int id);

        /// <summary>
        /// Create a new UserDTO.
        /// </summary>
        /// <param name="userDto">The new UserDTO to create.</param>
        IHttpActionResult Post([FromBody] UserDTO userDto);

        /// <summary>
        /// Update the UserDTO with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the UserDTO to update.</param>
        /// <param name="userDto">The new UserDTO data.</param>
        IHttpActionResult Put(int id, [FromBody] UserDTO userDto);

        /// <summary>
        /// Delete the UserDTO with the specified ID.
        /// A UserDTO can not be deleted when the UserDTO is participating in a study.
        /// </summary>
        /// <param name="id">The ID of the UserDTO to delete.</param>
        IHttpActionResult Delete(int id);
    }
}
