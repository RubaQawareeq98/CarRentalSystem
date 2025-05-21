using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Db.Repositories;

public class CarRepository(CarRentalSystemDbContext context) : ICarRepository
{
    public async Task<List<Car>> GetCarsAsync()
    {
        return await context.Cars
            .ToListAsync();
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

        if (carSearchDto.MinYear.HasValue)
            query = query.Where(c => c.Year >= carSearchDto.MinYear.Value);

        if (carSearchDto.MaxYear.HasValue)
            query = query.Where(c => c.Year <= carSearchDto.MaxYear.Value);

        query = query.Where(c => c.IsAvailable);

        return await query.ToListAsync();
    }
}