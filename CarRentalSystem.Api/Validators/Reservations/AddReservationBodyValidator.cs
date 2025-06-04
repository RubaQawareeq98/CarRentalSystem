using CarRentalSystem.Api.Models.Reservations;
using FluentValidation;

namespace CarRentalSystem.Api.Validators.Reservations;

public class AddReservationBodyValidator : AbstractValidator<AddReservationBodyDto>
{
    public AddReservationBodyValidator()
    {
        RuleFor(x => x.CarId)
            .NotEmpty()
            .WithMessage("Car Id cannot be empty")
            .WithErrorCode("CAR_ID_EMPTY")
            .WithSeverity(Severity.Error);
        
        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date cannot be empty");
        
        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date cannot be empty");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id cannot be empty")
            .WithErrorCode("USER_ID_EMPTY")
            .WithSeverity(Severity.Error);
    }
}
