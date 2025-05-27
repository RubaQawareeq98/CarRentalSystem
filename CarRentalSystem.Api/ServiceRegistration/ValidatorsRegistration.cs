using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Api.Models.Reservations;
using CarRentalSystem.Api.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class ValidatorsRegistration
{
    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        
        services.AddScoped<IValidator<SignupRequestBodyDto>, SignupRequestBodyValidator>();
        services.AddScoped<IValidator<CarRequestDto>, CarRequestBodyValidator>();
        services.AddScoped<IValidator<AddReservationBodyDto>, AddReservationBodyValidator>();
    }
}
