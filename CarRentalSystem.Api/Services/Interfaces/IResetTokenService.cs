using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Api.Services.Interfaces;

public interface IResetTokenService
{
    Task<string> AddResetPasswordToken(string email);
    Task<PasswordResetToken?> GetResetTokenAsync(string email, string token);
}