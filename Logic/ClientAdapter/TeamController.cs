using System;
using System.Web;
using System.Web.Http;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;
using Logic.Model;
using Logic.Model.DTO;

namespace Logic.ClientAdapter
{
    class TeamController : ApiController
    {
        [HttpGet]
        [ResponseType(typeof(TeamDto))]
        public async Task<IHttpActionResult> GetTeam(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<TeamDto>))]
        public async Task<IHttpActionResult> SearchTeams(String name)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateTeam(int id, TeamDto team)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ResponseType(typeof(Team))]
        public async Task<IHttpActionResult> AddTeam(TeamDto team)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> RemoveTeam(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
