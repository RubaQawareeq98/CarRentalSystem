using CarRentalSystem.Api.Models.Authentication;
using FluentValidation;

namespace CarRentalSystem.Api.Validators;

public class SignupRequestBodyValidator : AbstractValidator<SignupRequestBodyDto>
{
    public SignupRequestBodyValidator()
    {
        RuleFor(s => s.FirstName)
            .NotEmpty()
            .WithMessage("First name is required");
        
        RuleFor(s => s.LastName)
            .NotEmpty()
            .WithMessage("Last name is required");
        
        RuleFor(s => s.Email)
            .NotEmpty()
            .WithMessage("Email is required");
        
        RuleFor(s => s.Password)
            .NotEmpty()
            .WithMessage("Password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long");
        
        RuleFor(s => s.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password is required")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long");
        
        RuleFor(s => s.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required");
        
        RuleFor(s => s.City)
            .NotEmpty()
            .WithMessage("City is required");
        
        RuleFor(s => s.Country)
            .NotEmpty()
            .WithMessage("Country is required");
        
        RuleFor(s => s.AddressLine1)
            .NotEmpty()
            .WithMessage("Address line 1 is required");
    }
}
