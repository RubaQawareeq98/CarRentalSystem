using CarRentalSystem.Api.Mappers.Users;
using CarRentalSystem.Api.Models.Profile;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[Route("api/profile")]
[ApiController]
public class UserController(IUserRepository userRepository, UserProfileMapper mapper) : ControllerBase
{
    [HttpGet("{userId}")]
    [Authorize]
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
    [Authorize]
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
}
