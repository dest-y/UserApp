using Microsoft.AspNetCore.Mvc;
using UserApp.Services.UserService;

namespace UserApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        [Route("/Page/{page}")]
        public async Task<ActionResult<List<User>>> GetAllUsers(string? searchString = null, string? sortOrder = null, int page = 1)
        {
            var result = await _userService.GetAllUsers(searchString, sortOrder, page);

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var result = await _userService.GetUser(id);
            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            var result = await _userService.AddUser(user);

            return Ok(result);
        }

        [HttpPost]
        [Route("Roles/{UserId}/{RoleId}")]
        public async Task<ActionResult<List<Role>>> AddRole(int UserId, int RoleId)
        {
            var result = await _userService.AddRole(UserId, RoleId);
            if (result == null)
                return NotFound("User or Role not found");

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<List<User>>> UpdateUser(int id, User request)
        {
            var result = await _userService.UpdateUser(id, request);
            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<List<User>>> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }
    }
}
