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
            catch (ApplicationException appEx)
            {
                _logger.LogWarning(appEx, "Registration failed for email: {Email}", registerUserDTO.Email);
                return BadRequest(new { message = appEx.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while registering the user.");
                return StatusCode(500, new { message = "An unexpected error occurred during registration." });
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
        public async Task<IActionResult> GetUserProfileByEmail([FromQuery] string email)
        {
            try
            {
                if (string.IsNullOrEmpty(email))
                {
                    return BadRequest("Email is required.");
                }

                var user = await _userService.GetUserProfileByEmailAsync(email);
                if (user == null)
                {
                    return NotFound("User not found.");
                }

                var userDTO = new UserDTO
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                };

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user profile.");
                return StatusCode(500, new { message = "An error occurred while fetching the profile." });
            }
        }


    }
}
