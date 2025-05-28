using CarRentalSystem.Api.Models.Users;
using FluentValidation;

namespace CarRentalSystem.Api.Validators.Users;

public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
{
    public ResetPasswordDtoValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage("Email is required");
        
        RuleFor(r => r.Password)
            .NotEmpty()
            .WithMessage("Password is required");
        
        RuleFor(r => r.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Password is required");
        
        RuleFor(r => r.Token)
            .NotEmpty()
            .WithMessage("Token is required");
    }
}