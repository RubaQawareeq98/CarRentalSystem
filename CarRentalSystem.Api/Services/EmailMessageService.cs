using CarRentalSystem.Api.Services.Interfaces;

namespace CarRentalSystem.Api.Services;

public class EmailMessageService : IEmailMessageService
{
    public string GenerateResetPasswordEmail(string userEmail)
    {
        var resetLink = "https://localhost:5001/ResetPassword";
        return $"""
                    <h3>Password Reset Request</h3>
                    <p>Hi,</p>
                    <p>You requested to reset your password. Click the link below to proceed:</p>
                    <p><a href='{resetLink}'>Reset your password</a></p>
                    <p>This link will expire soon. If you didn't request this, you can safely ignore it.</p>
                    <br/>
                    <p>Thanks,<br/>Car Rental System Team</p>
                """;
    }
}