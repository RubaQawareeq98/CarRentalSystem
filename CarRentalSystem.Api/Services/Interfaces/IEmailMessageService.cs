namespace CarRentalSystem.Api.Services.Interfaces;

public interface IEmailMessageService
{
    string GenerateResetPasswordEmail(string userEmail, string resetLink);
}