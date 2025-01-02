using boilerplate_app.Application.DTOs;
using boilerplate_app.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace boilerplate_app.Application.Services
{
    public interface IJwtService
    {
        public string GenerateJwtToken(IdentityUser user, IList<string> roles);
    }
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(IdentityUser user, IList<string> roles)
        {
            var keyValue = _configuration["JwtSettings:Key"];
            if (string.IsNullOrEmpty(keyValue))
            {
                throw new ArgumentNullException("JWT Key is missing in configuration.");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyValue));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var userName = user.UserName ?? throw new ArgumentNullException("User name is missing.");
            var email = user.Email ?? throw new ArgumentNullException("Email is missing.");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email)
            };

            // Add roles to claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var expireMinutesStr = _configuration["JwtSettings:ExpireMinutes"];
            if (!double.TryParse(expireMinutesStr, out double expireMinutes))
            {
                throw new ArgumentException("Invalid expiration time in configuration.");
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
