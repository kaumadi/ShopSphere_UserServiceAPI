using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Domain.Models;

namespace UserService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);

    }
}
