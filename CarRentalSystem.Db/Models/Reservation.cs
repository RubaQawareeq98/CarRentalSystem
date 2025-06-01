namespace CarRentalSystem.Db.Models;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public Car? Car { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
