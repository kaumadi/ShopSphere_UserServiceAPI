using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTO;
using UserService.Application.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IJwtService jwtService, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task RegisterUserAsync(RegisterUserDTO registerUserDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to register user with email: {Email}", registerUserDTO.Email);

                var user = new User
                {
                    Name = registerUserDTO.Name,
                    Email = registerUserDTO.Email,
                    PasswordHash = "hashedpassword"  
                };

                await _userRepository.AddUserAsync(user);

                _logger.LogInformation("User successfully registered with email: {Email}", registerUserDTO.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while registering user with email: {Email}", registerUserDTO.Email);
                throw new ApplicationException("An error occurred while registering the user. Please try again later.");
            }
        }

        public async Task<UserDTO> AuthenticateUserAsync(AuthenticateUserDTO authenticateUserDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to authenticate user with email: {Email}", authenticateUserDTO.Email);

                var user = await _userRepository.GetUserByEmailAsync(authenticateUserDTO.Email);

                if (user == null || authenticateUserDTO.Password != "correctpassword") 
                {
                    _logger.LogWarning("Authentication failed for user with email: {Email}", authenticateUserDTO.Email);
                    return null; 
                }

                _logger.LogInformation("User successfully authenticated with email: {Email}", authenticateUserDTO.Email);

                return new UserDTO
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = "User" 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating user with email: {Email}", authenticateUserDTO.Email);
                throw new ApplicationException("An error occurred while authenticating the user. Please try again later.");
            }
        }


        public async Task<string> GenerateJwtTokenAsync(UserDTO userDTO)
        {
            try
            {
                _logger.LogInformation("Generating JWT token for user: {Email}", userDTO.Email);

                var token = "generated-jwt-token"; 
                return await Task.FromResult(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating JWT token for user: {Email}", userDTO.Email);
                throw new ApplicationException("An error occurred while generating the JWT token. Please try again later.");
            }
        }
    }
}
