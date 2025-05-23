using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Db.Repositories;

public class ReservationRepository(CarRentalSystemDbContext context) : IReservationRepository
{
    public async Task AddReservationAsync(Reservation reservation)
    {
        await context.Reservations.AddAsync(reservation);
        await context.SaveChangesAsync();
    }

    public async Task UpdateReservationAsync(Reservation reservation)
    {
        context.Reservations.Update(reservation);
        await context.SaveChangesAsync();
    }

    public async Task<List<Reservation>> GetUserReservationsAsync(Guid userId, int pageNumber, int pageSize)
    {
        return await context.Reservations
            .Where(r => r.UserId == userId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Reservation>> GetAllReservationsAsync()
    {
        return await context.Reservations.ToListAsync();
    }
}
