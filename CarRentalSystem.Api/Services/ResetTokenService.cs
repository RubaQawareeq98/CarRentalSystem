using System.Security.Cryptography;
using System.Text;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.AspNetCore.WebUtilities;

namespace CarRentalSystem.Api.Services;

public class ResetTokenService(IResetTokenRepository resetTokenRepository) : IResetTokenService
{
    public async Task<string> AddResetPasswordToken(string email)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        
        var resetToken = new PasswordResetToken
        {
            Email = email,
            Token = token,
            ExpiryDate = DateTime.UtcNow.AddMinutes(15)
        };
        
        await resetTokenRepository.SaveResetTokenAsync(resetToken);
        
        return encodedToken;
    }

    public async Task<PasswordResetToken?> GetResetTokenAsync(string email, string token)
    {
        return await resetTokenRepository.GetResetTokenAsync(email, token);
    }
}