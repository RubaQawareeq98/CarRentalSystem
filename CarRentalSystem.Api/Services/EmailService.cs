using brevo_csharp.Api;
using brevo_csharp.Model;
using CarRentalSystem.Api.Configurations;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

namespace CarRentalSystem.Api.Services;

public class EmailService(
    ILogger<EmailService> logger,
    IOptions<BrevoSettings> options,
    IOptions<ClientConfigurations> clientConfigurations,
    IEmailMessageService emailMessageService,
    IResetTokenService resetTokenService) : IEmailService
{
    private readonly BrevoSettings _brevoSettings = options.Value;
    private readonly ClientConfigurations _clientConfigurations = clientConfigurations.Value;
    
    public async Task SendResetPasswordEmail(User user)
    {
        logger.LogInformation("Sending reset password email");
        brevo_csharp.Client.Configuration.Default.AddApiKey("api-key", _brevoSettings.ApiKey);
        
        var encodedToken = resetTokenService.AddResetPasswordToken(user.Email);

        var resetLink = $"{_clientConfigurations.ClientUrl}reset-password?email={user.Email}&token={encodedToken}";

        var htmlContent =  emailMessageService.GenerateResetPasswordEmail(user.Email, resetLink);

        var apiInstance = new TransactionalEmailsApi();
        var sender = new SendSmtpEmailSender(_brevoSettings.SenderName, _brevoSettings.SenderEmail);

        var receiver = new SendSmtpEmailTo(user.Email, user.FirstName);
        var to = new List<SendSmtpEmailTo> { receiver };
        try
        {
            var sendSmtpEmail = new SendSmtpEmail(sender, to, null, null, htmlContent, null, "Reset Password.");
            
            await apiInstance.SendTransacEmailAsync(sendSmtpEmail);

            logger.LogDebug("Email sent to reset password");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}