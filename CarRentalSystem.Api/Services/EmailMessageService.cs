using CarRentalSystem.Api.Services.Interfaces;

namespace CarRentalSystem.Api.Services;

public class EmailMessageService : IEmailMessageService
{
    public string GenerateResetPasswordEmail(string userEmail)
    {
        const string resetLink = "https://localhost:5001/ResetPassword";
        return $"""
                    <h3>Password Reset Request</h3>
                    <p>Hi,</p>
                    <p>You requested to reset your password. Click the link below to proceed:</p>
                    <p><a href='{resetLink}'>Reset your password</a></p>
                    <br/>
                """;
    }
}