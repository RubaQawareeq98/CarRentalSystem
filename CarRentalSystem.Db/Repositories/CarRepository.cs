using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace CarRentalSystem.Db.Repositories;

public class CarRepository(CarRentalSystemDbContext context, ISieveProcessor sieveProcessor) : ICarRepository
{
    public async Task<List<Car>> GetCarsAsync(int pageNumber, int pageSize)
    {
        return await context.Cars
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Car>> GetAvailableCarsAsync(int pageNumber, int pageSize)
    {
        var today = DateTime.Today;

        var availableCars = await context.Cars
            .Where(c => c.Reservations != null && !c.Reservations.Any(r =>
                today >= r.StartDate && today <= r.EndDate
            ))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return availableCars;
    }

    public async Task<List<Car>> GetFilteredCarsAsync(CarSearchDto carSearchDto)
    {
        var cars = context.Cars.AsQueryable();

        // Apply filtering, sorting, and pagination
        cars = sieveProcessor.Apply(carSearchDto, cars);

        return await cars.ToListAsync();
    }

    public async Task<bool> IsCarAvailable(Guid carId, DateTime startDate, DateTime endDate)
    {
        var car = await context.Cars
            .Where(c => c.Id == carId &&
                        c.Reservations != null &&
                        !c.Reservations.Any(r =>
                            startDate >= r.StartDate && endDate <= r.EndDate
            ))
            .FirstOrDefaultAsync();
        return car is null;
    }

    public async Task AddCarAsync(Car car)
    {
        await context.Cars.AddAsync(car);
        await context.SaveChangesAsync();
    }

    public async Task UpdateCarAsync(Car car)
    {
        context.Cars.Update(car);
        await context.SaveChangesAsync();
    }

    public async Task<bool> IsCarExist(Guid carId)
    {
        return await context.Cars.AnyAsync(c => c.Id == carId);
    }
}