namespace CarRentalSystem.Api.Models.Reservations;

public class ReservationResponseDto
{
    public Guid Id { get; set; }
    public Guid CarId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid UserId { get; set; }
}
