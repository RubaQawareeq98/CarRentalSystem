using CarRentalSystem.Api.Configurations;
using CarRentalSystem.Api.Mappers.Authentication;
using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController(
    IUserRepository userRepository,
    IJwtTokenGeneratorService jwtTokenGeneratorService,
    JwtConfiguration jwtConfiguration,
    SignupRequestMapper mapper) : ControllerBase
{

    [HttpPost("sign-in")]
    public async Task<ActionResult> Login([FromBody] LoginRequestBodyDto request)
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

    [HttpPost("sign-up")]
    public async Task<ActionResult<string>> Signup(SignupRequestBodyDto request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return BadRequest("Passwords do not match.");
        }
        
        var existingUser = await userRepository.FindUserByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return BadRequest("Email is already token");
        }

        var user = mapper.ToUser(request);
        await userRepository.AddUserAsync(user);

        return Ok("Users Created Successfully");
    }
}
