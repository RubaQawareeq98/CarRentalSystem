using CarRentalSystem.Db.Models;
using Sieve.Models;

namespace CarRentalSystem.Db.Repositories.Interfaces;

public interface ICarRepository
{
    Task<List<Car>> GetCarsAsync(SieveModel sieveModel);
    Task<List<Car>> GetAvailableCarsAsync(SieveModel sieveModel);
    Task<List<Car>> GetFilteredCarsAsync(CarSearchDto carSearchDto);
    Task<bool> IsCarAvailable(Guid carId, DateTime startDate, DateTime endDate);
    Task AddCarAsync(Car? car);
    Task UpdateCarAsync(Car? car);
    Task<bool> IsCarExist(Guid carId);
    Task<Car?> GetCarById(Guid id);
}