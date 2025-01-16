using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTO;
using UserService.Domain.Models;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDTO registerUserDTO);
        Task<UserDTO> AuthenticateUserAsync(AuthenticateUserDTO authenticateUserDTO);
        Task<string> GenerateJwtTokenAsync(UserDTO userDTO);
        Task<User> GetUserProfileByEmailAsync(string email);
    }
}
