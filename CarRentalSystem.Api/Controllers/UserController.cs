using CarRentalSystem.Api.Mappers.Users;
using CarRentalSystem.Api.Models.Profile;
using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarRentalSystem.Api.Controllers;

[Authorize]
[Route("api/users")]
[ApiController]
public class UserController(IUserService userService,
    UserProfileMapper mapper,
    UserResponseMapper userResponseMapper) : ControllerBase
{
    [HttpGet("profile/{userId}")]
    public async Task<ActionResult<User>> GetUser(Guid userId)
    {
        var user = await userService.GetUserByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }

        var userProfile = mapper.ToProfileResponseDto(user);
        return Ok(userProfile);
    }

    [HttpPut("profile/{userId}")]
    public async Task<ActionResult> UpdateProfile(Guid userId, UpdateProfileBodyDto bodyDto)
    {
        var user = await userService.GetUserByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }

        user = mapper.UpdateUser(bodyDto);
        await userService.UpdateUserAsync(user);
        return Ok("Date updated successfully");
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers([FromQuery] SieveModel sieveModel)
    {
        var users = await userService.GetAllUsersAsync(sieveModel);
        var usersResponse = userResponseMapper.ToUserResponseDtos(users);
        return Ok(usersResponse);
    }
}
