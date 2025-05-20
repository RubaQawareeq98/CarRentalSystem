using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Api.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class AddValidators
{
    public static void RegisterValidators(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        
        services.AddScoped<IValidator<SignupRequestBodyDto>, SignupRequestBodyValidator>();
    }
}
