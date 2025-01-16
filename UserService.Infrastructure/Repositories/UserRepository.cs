using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UserService.Application.Interfaces;
using UserService.Domain.Models;
using UserService.Infrastructure.DbContexts;

namespace UserService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                _logger.LogInformation("Fetching the user details for the provided email: {Email}", email);

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    _logger.LogWarning("No user found with email: {Email}", email);
                    return new User(); 
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving user with email: {Email}", email);
                throw new ApplicationException("An error occurred while retrieving the user. Please try again later.");
            }
        }


        public async Task AddUserAsync(User user)
        {
            try
            {
                _logger.LogInformation("Adding user with email: {Email}", user.Email);
                if (string.IsNullOrEmpty(user.Role))
                {
                    user.Role = "User"; 
                }
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User added successfully with email: {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding user with email: {Email}", user.Email);
                throw new ApplicationException("An error occurred while adding the user.");
            }
        }

    }
}
