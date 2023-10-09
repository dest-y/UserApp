using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using UserApp.Auth;

namespace UserApp.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <remarks><br>Авторизация доступна только для mock-пользователя 'AuthUser'</br>
        /// </remarks>
        /// <response code="200">OK</response>
        /// <response code="401">Неверный пользователь</response>
        [HttpGet]
        [Route("/login")]
        public async Task<IActionResult> Index(string username)
        {
            if (username == "AuthUser")
            { 
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return Ok (new JwtSecurityTokenHandler().WriteToken(jwt));
            }
            return StatusCode(401, "Wrong Username");
        }
    }
}
