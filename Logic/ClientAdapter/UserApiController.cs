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
    public class UserApiController : ApiController
    {

        [HttpGet]
        [ResponseType(typeof(UserDto))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
             throw new NotImplementedException();
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<UserDto>))]
        public async Task<IHttpActionResult> SearchUsers(String name)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateUser(int id, UserDto user)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> AddUser(UserDto user)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> RemoveUser(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        
    }
}
