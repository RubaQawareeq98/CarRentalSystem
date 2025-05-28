using CarRentalSystem.Api.Models.Users;
using FluentValidation;

namespace CarRentalSystem.Api.Validators.Users;

public class ForgotPasswordValidator : AbstractValidator<ForgotPasswordDto>
{
    public ForgotPasswordValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email address is required");
    }
}