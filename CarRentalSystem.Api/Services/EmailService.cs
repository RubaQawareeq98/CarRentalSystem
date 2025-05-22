using brevo_csharp.Api;
using brevo_csharp.Model;
using CarRentalSystem.Api.Configurations;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

namespace CarRentalSystem.Api.Services;

public class EmailService(ILogger<EmailService> logger,
    IOptions<BrevoSettings> options,
    IEmailMessageService emailMessageService) : IEmailService
{
    private readonly BrevoSettings _brevoSettings = options.Value;
    
    public async Task SendResetPasswordEmail(User user)
    {
        var htmlContent =  emailMessageService.GenerateResetPasswordEmail(user.Email);

        var apiInstance = new TransactionalEmailsApi();
        var sender = new SendSmtpEmailSender(_brevoSettings.SenderName, _brevoSettings.SenderEmail);

        var receiver = new SendSmtpEmailTo(user.Email, user.FirstName);
        var to = new List<SendSmtpEmailTo> { receiver };

        try
        {
            var sendSmtpEmail = new SendSmtpEmail(sender, to, null, null, htmlContent, null, "Reset Password.");
            var result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
            logger.LogDebug(result.ToJson(), "email result {email}");
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
    }
}