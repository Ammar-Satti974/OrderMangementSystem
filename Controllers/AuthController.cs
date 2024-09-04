using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiProject.DAL;
using WebApiProject.Models;
using WebApiProject.ServiceLayer;

namespace WebApiProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // POST: api/Account/Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            return await ExecuteAsync(async () =>
            {

                if (user == null || string.IsNullOrEmpty(user.PasswordHash))
                    return BadRequest("Invalid registration data.");

                var users = new User
                {
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                };

                // Register the user and get the JWT token
                var token = await _userService.RegisterUserAsync(users, user.PasswordHash);
                if (token == null)
                    return BadRequest("User registration failed");
                return Ok(new { Token = token });
            });

        }

        // POST: api/Account/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            return await ExecuteAsync(async () =>
            {
                if (user == null || string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.PasswordHash))
                {
                    return BadRequest("Invalid login data.");
                }

                var token = await _userService.LoginUserAsync(user.Username, user.PasswordHash);
                if (token == null)
                    return Unauthorized("Invalid username or password");

                return Ok(new { Token = token });
            });
        }
    }
}
