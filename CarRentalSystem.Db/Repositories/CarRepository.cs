using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Db.Repositories;

public class CarRepository(CarRentalSystemDbContext context) : ICarRepository
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
        var query = context.Cars.AsQueryable();

        if (!string.IsNullOrEmpty(carSearchDto.Brand))
            query = query.Where(c => c.Brand.Contains(carSearchDto.Brand));

        if (!string.IsNullOrEmpty(carSearchDto.Model))
            query = query.Where(c => c.Model.Contains(carSearchDto.Model));

        if (!string.IsNullOrEmpty(carSearchDto.Location))
            query = query.Where(c => c.Location.Contains(carSearchDto.Location));

        if (carSearchDto.MinPrice.HasValue)
            query = query.Where(c => c.Price >= carSearchDto.MinPrice.Value);

        if (carSearchDto.MaxPrice.HasValue)
            query = query.Where(c => c.Price <= carSearchDto.MaxPrice.Value);
        
        if (!string.IsNullOrEmpty(carSearchDto.Color))
            query = query.Where(c => c.Color == carSearchDto.Color);

        if (carSearchDto.MinYear.HasValue)
            query = query.Where(c => c.Year >= carSearchDto.MinYear.Value);

        if (carSearchDto.MaxYear.HasValue)
            query = query.Where(c => c.Year <= carSearchDto.MaxYear.Value);
        
        if (carSearchDto is { StartDate: not null, EndDate: not null })
        {
            var startDate = carSearchDto.StartDate.Value.Date;
            var endDate = carSearchDto.EndDate.Value.Date;

            query = query.Where(c => c.Reservations != null && !c.Reservations.Any(r =>
                (startDate <= r.EndDate && endDate >= r.StartDate)
            ));
        }

        query = query.Where(c => c.IsAvailable);

        return await query.ToListAsync();
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