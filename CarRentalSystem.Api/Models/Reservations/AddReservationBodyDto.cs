namespace CarRentalSystem.Api.Models.Reservations;

public class AddReservationBodyDto
{
    public Guid CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid UserId { get; set; }
}