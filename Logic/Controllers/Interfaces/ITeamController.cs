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
        IEnumerable<Team> Get(string name = "");

        /// <summary>
        /// Get the team with the specific ID.
        /// </summary>
        /// <param name="id">The ID of the team to retrieve.</param>
        Team Get(int id);

        /// <summary>
        /// Create a new Team.
        /// </summary>
        /// <param name="team">The new team to create.</param>
        IHttpActionResult Post([FromBody] Team team);

        /// <summary>
        /// Update the team with the specified ID.
        /// The list of users part of the team can not be modified once it has been created.
        /// </summary>
        /// <param name="id">The ID of the team to update.</param>
        /// <param name="user">The new team data.</param>
        IHttpActionResult Put(int id, [FromBody] Team user);

        /// <summary>
        /// Delete the team with the specified ID.
        /// A team can not be deleted when it is participating in a study.
        /// </summary>
        /// <param name="id">The ID of the team to delete.</param>
        IHttpActionResult Delete(int id);
    }
}
