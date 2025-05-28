using CarRentalSystem.Api.Configurations;
using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(
    IUserService userService,
    IJwtTokenGeneratorService jwtTokenGeneratorService,
    JwtConfiguration jwtConfiguration) : ControllerBase
{

    [HttpPost("sign-in")]
    public async Task<ActionResult> Login([FromBody] LoginRequestBodyDto request)
    {
        var user = await userService.AuthenticateUserAsync(request.Email, request.Password);
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

    [HttpPost("sign-up")]
    public async Task<ActionResult<string>> Signup(SignupRequestBodyDto request)
    {
        var (success, message) = await userService.SignupAsync(request);

        if (!success)
        {
            return BadRequest(message);
        }

        return Ok(message);
    }
}
