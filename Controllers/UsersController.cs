using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text.Json;
using UserApp.Models;
using UserApp.Services.UserService;

namespace UserApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("/Page/{page}")]
        public async Task<ActionResult<List<User>>> GetAllUsers(string? searchString = null, string? sortOrder = null, int page = 1)
        {
            var routeInfo = ControllerContext.RouteData.Values.ToString();
            var result = await _userService.GetAllUsers(searchString, sortOrder, page);
            Log.Information($"GetAllUsers '{searchString}', '{sortOrder}', result = '{JsonSerializer.Serialize(result)}'");
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
        [Authorize]
        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            var result = await _userService.AddUser(user);

            return Ok(result);
        }

        [HttpPost]
        [Route("Roles/{UserId}/{RoleId}")]
        [Authorize]
        public async Task<ActionResult<List<Role>>> AddRole(int UserId, int RoleId)
        {
            var result = await _userService.AddRole(UserId, RoleId);
            if (result == null)
                return NotFound("User or Role not found");

            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<List<User>>> UpdateUser(int id, User request)
        {
            var result = await _userService.UpdateUser(id, request);
            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize]
        public async Task<ActionResult<List<User>>> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }
    }
}
