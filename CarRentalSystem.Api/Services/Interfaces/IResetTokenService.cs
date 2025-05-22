namespace CarRentalSystem.Api.Services.Interfaces;

public interface IResetTokenService
{
    Task<string> AddResetPasswordToken(string email);
}