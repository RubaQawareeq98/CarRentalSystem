using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace CarRentalSystem.Db.Repositories;

public class ReservationRepository(CarRentalSystemDbContext context, ISieveProcessor sieveProcessor) : IReservationRepository
{
    public async Task AddReservationAsync(Reservation? reservation)
    {
        await context.Reservations.AddAsync(reservation);
        await context.SaveChangesAsync();
    }

    public async Task UpdateReservationAsync(Reservation? reservation)
    {
        context.Reservations.Update(reservation);
        await context.SaveChangesAsync();
    }

    public async Task<List<Reservation?>> GetUserReservationsAsync(Guid userId, SieveModel sieveModel)
    {
        var query =  context.Reservations
            .Where(r => r != null && r.UserId == userId)
            .AsQueryable();
            
        query = sieveProcessor.Apply(sieveModel, query);
        return await query.ToListAsync();
    }

    public async Task<List<Reservation?>> GetAllReservationsAsync()
    {
        return await context.Reservations.ToListAsync();
    }

    public async Task<Reservation?> GetReservationByIdAsync(Guid id)
    {
        return await context.Reservations.FirstOrDefaultAsync(r => r != null && r.Id == id);
    }
}
