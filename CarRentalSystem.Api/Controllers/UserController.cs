using CarRentalSystem.Api.Mappers.Users;
using CarRentalSystem.Api.Models.Profile;
using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[Authorize]
[Route("api/users")]
[ApiController]
public class UserController(IUserRepository userRepository,
    UserProfileMapper mapper,
    UserResponseMapper userResponseMapper) : ControllerBase
{
    [HttpGet("profile/{userId}")]
    public async Task<ActionResult<User>> GetUser(Guid userId)
    {
        var user = await userRepository.FindUserByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }

        var userProfile = mapper.ToProfileResponseDto(user);
        return Ok(userProfile);
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult> UpdateProfile(Guid userId, UpdateProfileBodyDto bodyDto)
    {
        var user = await userRepository.FindUserByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }
        mapper.UpdateUser(bodyDto, user);
        
        await userRepository.UpdateUserAsync(user);
        return Ok("Date updated successfully");
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
    {
        var users = await userRepository.GetAllUsersAsync();
        var usersResponse = userResponseMapper.ToUserResponseDtos(users);
        return Ok(usersResponse);
    }
}
