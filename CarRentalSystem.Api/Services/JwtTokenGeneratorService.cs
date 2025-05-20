using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CarRentalSystem.Api.Configurations;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using Microsoft.IdentityModel.Tokens;

namespace CarRentalSystem.Api.Services;

public class JwtTokenGeneratorService(JwtConfiguration configuration) : IJwtTokenGeneratorService
{
    public Task<string> GenerateToken(User user)
    {
        
        var securityKey = new SymmetricSecurityKey(
            Convert.FromBase64String(configuration.SecretKey));
            
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        var claimsForToken = new List<Claim>
        {
            new("sub", user.Id.ToString()),
            new("email", user.Email),
            new("role", user.Role.ToString())
        };

        var jwt = new JwtSecurityToken(
            configuration.Issuer,
            configuration.Audience,
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(configuration.TokenExpirationMinutes),
            signingCredentials
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.WriteToken(jwt);
            
        return Task.FromResult(token);
    }
}
