namespace CarRentalSystem.Api.Models.Cars;

public class UpdateCarRequestDto
{
    public string? Brand { get; set; } 
    public string? Model { get; set; } 
    public string? Color { get; set; } 
    public int? Year { get; set; }
    public string? Location { get; set; } 
    public decimal? Price { get; set; }
}