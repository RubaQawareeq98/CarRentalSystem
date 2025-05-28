using CarRentalSystem.Api.Models.Cars;
using FluentValidation;

namespace CarRentalSystem.Api.Validators.Cars;

public class CarRequestBodyValidator : AbstractValidator<CarRequestDto>
{
    public CarRequestBodyValidator()
    {
        RuleFor(c => c.Location)
            .NotEmpty()
            .WithMessage("Location cannot be empty");
        
        RuleFor(c => c.Price)
            .NotEmpty()
            .GreaterThan(0)
            .WithMessage("Price cannot be empty");
        
        RuleFor(c => c.Color)
            .NotEmpty()
            .WithMessage("Color cannot be empty");
        
        RuleFor(c => c.Brand)
            .NotEmpty()
            .WithMessage("Brand cannot be empty");
        
        RuleFor(c => c.Model)
            .NotEmpty()
            .WithMessage("Model cannot be empty");
        
        RuleFor(c => c.Year)
            .NotEmpty()
            .WithMessage("Year cannot be empty");
    }
}