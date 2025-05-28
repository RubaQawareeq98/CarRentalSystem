using CarRentalSystem.Api.Mappers.Reservations;
using CarRentalSystem.Api.Models.Reservations;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Repositories.Interfaces;
using Sieve.Models;

namespace CarRentalSystem.Api.Services;

public class ReservationService(
    IReservationRepository reservationRepository,
    ICarService carService,
    AddReservationMapper mapper,
    UpdateReservationMapper updateReservationMapper)
    : IReservationService
{
    public async Task<bool> AddReservationAsync(AddReservationBodyDto bodyDto)
    {
        var isCarAvailable = await carService.IsCarAvailableAsync(bodyDto.CarId, bodyDto.StartDate, bodyDto.EndDate);
        if (!isCarAvailable)
            return false;

        var reservation = mapper.ToReservation(bodyDto);
        await reservationRepository.AddReservationAsync(reservation);
        return true;
    }

    public async Task<List<ReservationResponseDto>> GetUserReservationsAsync(Guid userId, SieveModel sieveModel)
    {
        var reservations = await reservationRepository.GetUserReservationsAsync(userId, sieveModel);
        return mapper.ToReservationResponseDtos(reservations);
    }

    public async Task<bool> UpdateReservationAsync(Guid reservationId, UpdateReservationDto updateDto)
    {
        var reservation = await reservationRepository.GetReservationByIdAsync(reservationId);
        if (reservation is null)
        {
            return false;
        }

        updateReservationMapper.UpdateReservation(updateDto, reservation);
        await reservationRepository.UpdateReservationAsync(reservation);
        return true;
    }

    public async Task<List<ReservationResponseDto>> GetAllReservationsAsync()
    {
        var reservations = await reservationRepository.GetAllReservationsAsync();
        return mapper.ToReservationResponseDtos(reservations);
    }
}
