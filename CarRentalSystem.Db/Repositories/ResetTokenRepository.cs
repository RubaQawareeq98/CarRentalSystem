using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Db.Repositories;

public class ResetTokenRepository(CarRentalSystemDbContext context) : IResetTokenRepository
{
    public async Task SaveResetTokenAsync(PasswordResetToken? token)
    {
        context.PasswordResetTokens.Add(token);
        await context.SaveChangesAsync();
    }

    public async Task<PasswordResetToken?> GetResetTokenAsync(string email, string token)
    {
       return await context.PasswordResetTokens
            .FirstOrDefaultAsync(t => t.Email == email && t.Token == token);
    }
}