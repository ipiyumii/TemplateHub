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
      
    }
}
