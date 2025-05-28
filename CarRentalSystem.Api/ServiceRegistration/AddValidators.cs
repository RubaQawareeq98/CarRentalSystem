using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Api.Models.Reservations;
using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Api.Validators;
using CarRentalSystem.Api.Validators.Cars;
using CarRentalSystem.Api.Validators.Reservations;
using CarRentalSystem.Api.Validators.Users;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class AddValidators
{
    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        
        services.AddScoped<IValidator<SignupRequestBodyDto>, SignupRequestBodyValidator>();
        services.AddScoped<IValidator<CarRequestDto>, CarRequestBodyValidator>();
        services.AddScoped<IValidator<AddReservationBodyDto>, AddReservationBodyValidator>();
        services.AddScoped<IValidator<LoginRequestBodyDto>, LoginRequestBodyValidator>();
        services.AddScoped<IValidator<ForgotPasswordDto>, ForgotPasswordValidator>();
        services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
    }
}
