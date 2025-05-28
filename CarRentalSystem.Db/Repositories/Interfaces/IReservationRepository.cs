using CarRentalSystem.Db.Models;
using Sieve.Models;

namespace CarRentalSystem.Db.Repositories.Interfaces;

public interface IReservationRepository
{
    Task AddReservationAsync(Reservation? reservation);
    Task UpdateReservationAsync(Reservation? reservation);
    Task<List<Reservation?>> GetUserReservationsAsync(Guid userId, SieveModel sieveModel);
    Task<List<Reservation?>> GetAllReservationsAsync();
    Task<Reservation?> GetReservationByIdAsync(Guid id);
}