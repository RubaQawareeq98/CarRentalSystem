using CarRentalSystem.Db.Enums;

namespace CarRentalSystem.Db.Models;

public class User
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }   
    public string PhoneNumber { get; set; } = string.Empty;
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string DriverLicenseNumber { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public virtual List<Reservation>? Reservations { get; set; }
}
