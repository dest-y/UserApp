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

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Получение списка всех пользователей и их ролей.
        /// </summary>
        /// <remarks><br>Реализован поиск по имени пользователя, Email и названии роли пользователя.</br>
        /// <br>Доступна фильтрацция:</br>
        /// <br>name_desc - по имени пользователя(DESC)</br>
        /// <br>age - по возрасту(ASC)</br>
        /// <br>email_desc - по Email(DESC)</br>
        /// <br>DEFAULT - по Имени пользователя(ASC) => по названию роли(ASC)</br> 
        /// <br>Пагинация - 5 пользователей в 1 запросе, страница по умолчанию 1</br> 
        /// </remarks>
        /// <response code="200">OK</response>
        [HttpGet]
        [Route("/Page/{page}")]
        public async Task<ActionResult<List<User>>> GetAllUsers(string? searchString = null, string? sortOrder = null, int page = 1)
        {
            var routeInfo = ControllerContext.RouteData.Values.ToString();
            var result = await _userService.GetAllUsers(searchString, sortOrder, page);
            return Ok(result);
        }

        /// <summary>
        /// Получение пользователя и всех его ролей по ID пользователя.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Пользователь не найден</response>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var result = await _userService.GetUser(id);
            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }
        /// <summary>
        /// Добавление нового пользователя, требуется авторизация.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="401">Требуется авторизация</response>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<List<User>>> AddUser(User user)
        {
            var result = await _userService.AddUser(user);

            return Ok(result);
        }

        /// <summary>
        /// Добавление роли существующему пользователю, требуется авторизация.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="401">Требуется авторизация</response>
        /// <response code="404">Пользователь или роль не найден</response>
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
        /// <summary>
        /// Изменение данных для уже существующего пользователя, требуется авторизация.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="401">Требуется авторизация</response>
        /// <response code="404">Пользователь не найден</response>
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
        /// <summary>
        /// Удаление пользователя, требуется авторизация.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="401">Требуется авторизация</response>
        /// <response code="404">Пользователь не найден</response>
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
