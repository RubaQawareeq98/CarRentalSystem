using CarRentalSystem.Api.Models.Reservations;
using CarRentalSystem.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarRentalSystem.Api.Controllers;

[Authorize]
[Route("api/reservations")]
[ApiController]
public class ReservationController(IReservationService reservationService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> AddReservation([FromBody] AddReservationBodyDto bodyDto)
    {
        var isSuccess = await reservationService.AddReservationAsync(bodyDto);
        return isSuccess ? Ok("Reservation Added Successfully"): BadRequest("Reservation Added Failed");
    }
    
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<ReservationResponseDto>>> GetUserReservations(Guid userId,[FromQuery] SieveModel sieveModel)
    {
        var reservations = await reservationService.GetUserReservationsAsync(userId, sieveModel);
        return Ok(reservations);
    }

    [HttpPut("reservation/{reservationId}")]
    public async Task<ActionResult> UpdateReservation(Guid reservationId, UpdateReservationDto dto)
    {
        var isSuccess = await reservationService.UpdateReservationAsync(reservationId, dto);
        return isSuccess? NoContent() : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<ReservationResponseDto>>> GetAllReservations()
    {
        var reservations = await reservationService.GetAllReservationsAsync();
        return Ok(reservations);
    }
}
