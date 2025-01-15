using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.DTO;
using UserService.Application.Interfaces;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegisterUserDTO registerUserDTO)
        {
            try
            {
                await _userService.RegisterUserAsync(registerUserDTO);
                return Ok(new { message = "User registered successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering the user.");
                return StatusCode(500, new { message = "An error occurred during registration." });
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateUser(AuthenticateUserDTO authenticateUserDTO)
        {
            try
            {
                var user = await _userService.AuthenticateUserAsync(authenticateUserDTO);
                if (user == null)
                {
                    _logger.LogWarning("Failed authentication attempt for username: {username}", authenticateUserDTO.Email);
                    return Unauthorized("Invalid credentials.");
                }

                var token = await _userService.GenerateJwtTokenAsync(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while authenticating the user.");
                return StatusCode(500, new { message = "An error occurred during authentication." });
            }
        }

        [HttpGet("profile")]
        [Authorize(Policy = "UserPolicy")]
        public IActionResult GetUserProfile()
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID not found in claims.");
                    return Unauthorized("User ID not found.");
                }

                return Ok(new { message = $"Hello {userId}, this is your profile data." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user profile.");
                return StatusCode(500, new { message = "An error occurred while fetching the profile data." });
            }
        }

        [HttpGet("admin")]
        [Authorize(Policy = "AdminPolicy")]
        public IActionResult GetAdminData()
        {
            try
            {
                return Ok(new { message = "Admin data accessed." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving admin data.");
                return StatusCode(500, new { message = "An error occurred while fetching admin data." });
            }
        }
    }
}
