using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;

namespace CarRentalSystem.Db.Repositories;

public class CarRepository(CarRentalSystemDbContext context, ISieveProcessor sieveProcessor) : ICarRepository
{
    public async Task<List<Car>> GetCarsAsync(SieveModel sieveModel)
    {
        var query = context.Cars.AsQueryable();
        query = sieveProcessor.Apply(sieveModel, query);

        return await query.ToListAsync();
    }


    public async Task<List<Car>> GetAvailableCarsAsync(SieveModel sieveModel)
    {
        var today = DateTime.Today;

        var availableCars = context.Cars
            .Where(c => c.Reservations != null && c.IsAvailable && !c.Reservations.Any(r =>
                today >= r.StartDate && today <= r.EndDate
            ))
            .AsQueryable();
        
        availableCars = sieveProcessor.Apply(sieveModel, availableCars);
        
        return await availableCars.ToListAsync();
    }

    public async Task<List<Car>> GetFilteredCarsAsync(CarSearchDto carSearchDto)
    {
        var cars = context.Cars.AsQueryable();

        cars = sieveProcessor.Apply(carSearchDto, cars);

        return await cars.ToListAsync();
    }

    public async Task<bool> IsCarAvailable(Guid carId, DateTime startDate, DateTime endDate)
    {
        return await context.Cars
            .Where(c => c.Id == carId)
            .AnyAsync(c => c.IsAvailable && 
                           !c.Reservations.Any(r => 
                               (startDate >= r.StartDate && startDate <= r.EndDate) ||  
                               (endDate >= r.StartDate && endDate <= r.EndDate) ||      
                               (r.StartDate >= startDate && r.StartDate <= endDate))); 
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

    public async Task<Car> GetCarById(Guid id)
    {
        return await context.Cars.FirstAsync(c => c.Id == id);
    }
}