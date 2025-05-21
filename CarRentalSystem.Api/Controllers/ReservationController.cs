using CarRentalSystem.Api.Mappers.Reservations;
using CarRentalSystem.Api.Models.Reservations;
using CarRentalSystem.Db.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[Route("api/reservations")]
[ApiController]
public class ReservationController(ReservationRepository reservationRepository, AddReservationMapper mapper) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddReservation([FromBody] AddReservationBodyDto bodyDto)
    {
        var reservation = mapper.ToReservation(bodyDto);
        await reservationRepository.AddReservationAsync(reservation);
        return Ok("Reservation added successfully");
    }
}