using CarRentalSystem.Api.Mappers.Reservations;
using CarRentalSystem.Api.Models.Reservations;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[Route("api/reservations")]
[ApiController]
public class ReservationController(IReservationRepository reservationRepository,
    ICarRepository carRepository,
    AddReservationMapper mapper) : ControllerBase
{
    private int _maxPageSize;

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> AddReservation([FromBody] AddReservationBodyDto bodyDto)
    {
        var isCarAvailable = await carRepository.IsCarAvailable(bodyDto.CarId, bodyDto.StartDate, bodyDto.EndDate);
        if (!isCarAvailable)
        {
            return BadRequest("Car is not available in the selected date");
        }
        
        var reservation = mapper.ToReservation(bodyDto);
        await reservationRepository.AddReservationAsync(reservation);
        return Ok("Reservation added successfully");
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<List<Reservation>>> GetUserReservations(Guid userId, int pageNumber, int pageSize)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be greater than 0");
        }
        
        _maxPageSize = Math.Min(_maxPageSize, pageSize);
        var reservations = await reservationRepository.GetUserReservationsAsync(userId, pageNumber, pageSize);
        
        var reservationsResponse = mapper.ToReservationResponseDtos(reservations);
        return Ok(reservationsResponse);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<ReservationResponseDto>>> GetAllReservations()
    {
        var reservations = await reservationRepository.GetAllReservationsAsync();
        var reservationsResponse = mapper.ToReservationResponseDtos(reservations);
        return Ok(reservationsResponse);
    }
}
