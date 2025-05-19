namespace CarRentalSystem.Db.Models;

public class Reservation
{
    public int Id { get; set; }
    public int CarId { get; set; }
    public Car? Car { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public long UserId { get; set; }
    public User? User { get; set; }
}
