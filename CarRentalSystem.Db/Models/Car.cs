using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Db.Models;

public class Car
{
    public Guid Id { get; set; }
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    
    [Precision(10,2)]
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public virtual List<Reservation>? Reservations { get; set; }
}
