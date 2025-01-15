using Microsoft.Extensions.Configuration;
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

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> GenerateJwtToken(UserDTO userDTO)
        {
            try
            {
                var claims = new[]
                {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, userDTO.Name),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, userDTO.Email),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, userDTO.Role),
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userDTO.Email), // unique identifier
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var issuer = _configuration["Jwt:Issuer"];
                var audience = _configuration["Jwt:Audience"];

                if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                {
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

                return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred when generating the JWT token.", ex);
            }
        }
    }
}
