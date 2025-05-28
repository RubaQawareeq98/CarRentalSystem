using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Api.Services.Interfaces;

namespace CarRentalSystem.Api.Services;

public class ResetPasswordService(IUserService userService,
    IEmailService emailService,
    IResetTokenService resetTokenService) : IResetPasswordService
{
    public async Task<bool> ForgotPassword(string email)
    {
        var user = await userService.GetUserByEmailAsync(email);
        if (user is null)
        {
            return false;
        }
            
        await emailService.SendResetPasswordEmail(user);
        return true;
    }

    public async Task<(bool Success, string Message)> ResetPasswordAsync(ResetPasswordDto dto)
    {
        var tokenEntry = await resetTokenService.GetResetTokenAsync(dto.Email, dto.Token);
        if (tokenEntry is null || tokenEntry.ExpiryDate < DateTime.UtcNow)
            return (false, "Invalid or expired token.");

        if (dto.Password != dto.ConfirmPassword)
            return (false, "Passwords do not match.");

        var user = await userService.GetUserByEmailAsync(dto.Email);
        if (user is null)
            return (false, "User not found.");

        await userService.UpdateUserAsync(user);

        return (true, "Password has been reset successfully.");
    }
}