namespace CarRentalSystem.Api.Models.Reservations;

public class UpdateReservationDto
{
    public Guid CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
