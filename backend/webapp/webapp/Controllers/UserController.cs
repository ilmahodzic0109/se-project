using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sunglasses.Services.Interfaces;
using webapp.dto;

namespace webapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto request)
        {
            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest("Passwords do not match.");
            }

            try
            {
                var user = await _userService.RegisterUserAsync(request.Email, request.Password);

                user.IsLogged = true;

                await _userService.UpdateUserAsync(user);

                return Ok(new
                {
                    userId = user.UserId,
                    isAdmin = user.IsAdmin,
                    isLogged = user.IsLogged
                });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Email already in use"))
                {
                    return BadRequest(new { message = "Email already in use" });
                }
                return BadRequest(new { message = "An error occurred during registration" });
            }

        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            try
            {
                var user = await _userService.LoginUserAsync(request.Email, request.Password);

                if (user != null && user.IsLogged)
                {
                    user.IsLogged = true;
                    await _userService.UpdateUserAsync(user);
                    return Ok(new { UserId = user.UserId, IsAdmin = user.IsAdmin, IsLogged = user.IsLogged });
                }
                return Unauthorized(new { message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDto request)
        {
            try
            {
                await _userService.LogoutUserAsync(request.UserId);
                return Ok(new { message = "User logged out successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }
    }
}
