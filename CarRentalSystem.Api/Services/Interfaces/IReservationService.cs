using CarRentalSystem.Api.Models.Reservations;
using Sieve.Models;

namespace CarRentalSystem.Api.Services.Interfaces;

public interface IReservationService
{
    Task<bool> AddReservationAsync(AddReservationBodyDto bodyDto);
    Task<List<ReservationResponseDto>> GetUserReservationsAsync(Guid userId, SieveModel sieveModel);
    Task<bool> UpdateReservationAsync(Guid reservationId, UpdateReservationDto updateDto);
    Task<List<ReservationResponseDto>> GetAllReservationsAsync();
}
