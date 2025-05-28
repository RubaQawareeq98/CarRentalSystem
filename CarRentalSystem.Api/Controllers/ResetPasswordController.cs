using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[Route("api/")]
[ApiController]
public class ResetPasswordController(IResetPasswordService resetPasswordService) : ControllerBase
{

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        var isSuccess = await resetPasswordService.ForgotPassword(forgotPasswordDto.Email);
        return isSuccess? Ok("Password reset link has been sent") : NotFound("Email address not found");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
    {
        var (success, message) = await resetPasswordService.ResetPasswordAsync(dto);
        return success ? Ok(message) : BadRequest(message);
    }
}