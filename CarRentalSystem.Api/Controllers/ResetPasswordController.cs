using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[Route("api/")]
[ApiController]
public class ResetPasswordController(IEmailService emailService,
    IUserRepository userRepository,
    IResetTokenRepository tokenRepository) : ControllerBase
{

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        var user = await userRepository.FindUserByEmailAsync(forgotPasswordDto.Email);
        if (user is null)
        {
            return NotFound();
        }
            
        await emailService.SendResetPasswordEmail(user);
        return Ok("Password reset link has been sent");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var tokenEntry = await tokenRepository.GetResetTokenAsync(resetPasswordDto.Email, resetPasswordDto.Token);
        if (tokenEntry is null || tokenEntry.ExpiryDate < DateTime.UtcNow)
        {
            return BadRequest("Invalid or expired token.");
        }

        
        if (resetPasswordDto.Password != resetPasswordDto.ConfirmPassword)
        {
            return BadRequest("Passwords do not match.");
        }
            
        var user = await userRepository.FindUserByEmailAsync(resetPasswordDto.Email);

        if (user is null)
        {
            return NotFound("User not found.");
        }

        user.Password = resetPasswordDto.Password;
        await userRepository.UpdateUserAsync(user);

        return Ok("Password has been reset successfully.");
    }
}