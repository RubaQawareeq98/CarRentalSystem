using Sieve.Attributes;

namespace CarRentalSystem.Db.Models;
public class Car
{
    public Guid Id { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Brand { get; set; } = string.Empty;
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Model { get; set; } = string.Empty;
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Color { get; set; } = string.Empty;
    
    [Sieve(CanFilter = true, CanSort = true)]
    public int Year { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public string Location { get; set; } = string.Empty;
    
    [Sieve(CanFilter = true, CanSort = true)]
    public decimal Price { get; set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public bool IsAvailable { get; set; } = true;
    public virtual List<Reservation>? Reservations { get; set; }
}
