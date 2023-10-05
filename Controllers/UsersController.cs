using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {

            var result = await _userService.GetUser(id);

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            var result = await _userService.AddUser(user);

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<List<User>>> UpdateUser(int id, User request)
        {

            var result = await _userService.UpdateUser(id, request);

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
