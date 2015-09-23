using System.Collections.Generic;
using System.Web.Http;
using Hunter.Services;
using Hunter.Services.Dto;

namespace Hunter.Rest.Controllers
{
    public class UserRoleController : ApiController
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        // GET: api/Role
        [HttpGet]
        public IEnumerable<UserRoleDto> Get()
        {
            return _userRoleService.GetAllUserRoles();
        }

        // GET: api/Role/5
        [HttpGet]
        public UserRoleDto Get(int id)
        {
            return _userRoleService.GetUserRoleById(id);
        }

        // POST: api/Role
        [HttpPost]
        public IHttpActionResult Post([FromBody]UserRoleDto userRoleDto)
        {
            if (_userRoleService.IsRoleExist(userRoleDto.Name))
            {
                return Conflict();
            }
            _userRoleService.CreateUserRole(userRoleDto);
            return Ok();

        }

        // PUT: api/Role/5
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]UserRoleDto userRoleDto)
        {
            if (!_userRoleService.IsRoleExist(id))
            {
                return NotFound();
            }
            _userRoleService.UpdateUserRole(userRoleDto);
            return Ok();

        }

        // DELETE: api/Role/5
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (!_userRoleService.IsRoleExist(id))
            {
                return NotFound();
            }
            _userRoleService.DeleteUserRole(id);
            return Ok();
        }
    }
}
