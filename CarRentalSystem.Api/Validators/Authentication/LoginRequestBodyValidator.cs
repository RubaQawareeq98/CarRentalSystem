using CarRentalSystem.Api.Models.Authentication;
using FluentValidation;

namespace CarRentalSystem.Api.Validators;

public class LoginRequestBodyValidator : AbstractValidator<LoginRequestBodyDto>
{
    public LoginRequestBodyValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("Email is required");
        
        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}