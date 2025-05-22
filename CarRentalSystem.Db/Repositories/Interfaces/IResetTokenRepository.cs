using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Db.Repositories.Interfaces;

public interface IResetTokenRepository
{
    Task SaveResetTokenAsync(PasswordResetToken? token);
    Task<PasswordResetToken?> GetResetTokenAsync(string email, string token);
}