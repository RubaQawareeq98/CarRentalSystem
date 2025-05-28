using CarRentalSystem.Api.Models.Users;

namespace CarRentalSystem.Api.Services.Interfaces;

public interface IResetPasswordService
{
    Task<bool> ForgotPassword(string email);
    Task<(bool Success, string Message)> ResetPasswordAsync(ResetPasswordDto dto);
}