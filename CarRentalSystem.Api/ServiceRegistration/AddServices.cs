using CarRentalSystem.Api.Services;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db;
using CarRentalSystem.Db.Repositories;
using CarRentalSystem.Db.Repositories.Interfaces;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class AddServices
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddDbContext<CarRentalSystemDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
        services.AddScoped<ICarRepository, CarRepository>();
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IEmailMessageService, EmailMessageService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IResetTokenService, ResetTokenService>();
        services.AddScoped<IResetTokenRepository, ResetTokenRepository>();
        services.AddScoped<IUserService, IUserService>();
        services.AddScoped<IResetPasswordService, ResetPasswordService>();
    }
}