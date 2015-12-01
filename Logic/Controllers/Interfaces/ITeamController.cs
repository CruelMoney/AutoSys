using System.Collections.Generic;
using System.Web.Http;
using Logic.Model.DTO;

namespace Logic.Controllers.Interfaces
{
    /// <summary>
    /// Controller to access and modify teams.
    /// </summary>
    public interface ITeamController
    {
        /// <summary>
        /// Get all teams.
        /// </summary>
        /// <param name="name">Search for teams which match the specified name.</param>
        IHttpActionResult Get(string name = "");

        /// <summary>
        /// Get the TeamDTO with the specific ID.
        /// </summary>
        /// <param name="id">The ID of the TeamDTO to retrieve.</param>
        IHttpActionResult Get(int id);

        /// <summary>
        /// Create a new TeamDTO.
        /// </summary>
        /// <param name="teamDto">The new TeamDTO to create.</param>
        IHttpActionResult Post([FromBody] TeamDTO teamDto);

        /// <summary>
        /// Update the TeamDTO with the specified ID.
        /// The list of users part of the TeamDTO can not be modified once it has been created.
        /// </summary>
        /// <param name="id">The ID of the TeamDTO to update.</param>
        /// <param name="user">The new TeamDTO data.</param>
        IHttpActionResult Put(int id, [FromBody] TeamDTO user);

        /// <summary>
        /// Delete the TeamDTO with the specified ID.
        /// A TeamDTO can not be deleted when it is participating in a study.
        /// </summary>
        /// <param name="id">The ID of the TeamDTO to delete.</param>
        IHttpActionResult Delete(int id);
    }
}
