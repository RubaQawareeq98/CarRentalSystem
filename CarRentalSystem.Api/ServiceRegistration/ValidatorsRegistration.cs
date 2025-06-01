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

public static class ValidatorsRegistration
{
    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddSingleton<IValidator<SignupRequestBodyDto>, SignupRequestBodyValidator>();
        services.AddSingleton<IValidator<CarRequestDto>, CarRequestBodyValidator>();
        services.AddSingleton<IValidator<AddReservationBodyDto>, AddReservationBodyValidator>();
        services.AddSingleton<IValidator<LoginRequestBodyDto>, LoginRequestBodyValidator>();
        services.AddSingleton<IValidator<ForgotPasswordDto>, ForgotPasswordValidator>();
        services.AddSingleton<IValidator<ResetPasswordDto>, ResetPasswordDtoValidator>();
    }
}
