using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Api.Services.Interfaces;

public interface IEmailService
{
    Task SendResetPasswordEmail(User user);
}