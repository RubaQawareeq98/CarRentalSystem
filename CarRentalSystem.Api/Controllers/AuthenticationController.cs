using CarRentalSystem.Api.Configurations;
using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(
    IUserRepository userRepository,
    IJwtTokenGeneratorService jwtTokenGeneratorService,
    JwtConfiguration jwtConfiguration) : ControllerBase
{

    [HttpPost("sign-in")]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
    {
        var user = await userRepository.FindUserByCredentials(request.Email, request.Password);
        if (user is null)
        {
            return Unauthorized("Email or password is incorrect.");
        }

        var token = await jwtTokenGeneratorService.GenerateToken(user);
        
        var response = new AuthResponseDto
        {
            Token = token,
            ExpirationInMinutes = jwtConfiguration.TokenExpirationMinutes
        };
        return Ok(response);
    }
}
