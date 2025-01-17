using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UserService.Application.DTO;
using UserService.Application.Interfaces;
namespace UserService.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtService> _logger;
        public JwtService(IConfiguration configuration, ILogger<JwtService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<string> GenerateJwtToken(UserDTO userDTO)
        {
            try
            {
                _logger.LogInformation("Generating JWT token for user: {Email}", userDTO.Email);

                var claims = new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, userDTO.Name),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, userDTO.Email),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, userDTO.Role),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userDTO.Email),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var issuer = _configuration["JwtSettings:Issuer"];
                var audience = _configuration["JwtSettings:Audience"];

                if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                {
                    _logger.LogError("JWT issuer or audience configuration is missing.");
                    throw new InvalidOperationException("Issuer and Audience are not configured correctly.");
                }

                var expirationTime = DateTime.UtcNow.AddHours(1);

                var token = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: claims,
                    expires: expirationTime,
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while generating JWT token for user: {Email}", userDTO.Email);
                throw new Exception("An error occurred when generating the JWT token.", ex);
            }
        }
    }
}
