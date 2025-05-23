using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Db.Repositories.Interfaces;

public interface IReservationRepository
{
    Task AddReservationAsync(Reservation reservation);
    Task UpdateReservationAsync(Reservation reservation);
    Task<List<Reservation>> GetUserReservationsAsync(Guid userId, int pageNumber, int pageSize);
    Task<List<Reservation>> GetAllReservationsAsync();
}