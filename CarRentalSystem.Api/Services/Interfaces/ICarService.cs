using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Db.Models;
using Sieve.Models;

namespace CarRentalSystem.Api.Services.Interfaces;

public interface ICarService
{
    Task<List<Car>> GetAllCarsAsync(SieveModel sieveModel);
    Task<List<Car>> GetAvailableCarsAsync(SieveModel sieveModel);
    Task<Car?> GetCarByIdAsync(Guid id);
    Task<List<Car>> SearchCarsAsync(CarSearchDto carSearchDto);
    Task<bool> IsCarAvailableAsync(Guid carId, DateTime startDate, DateTime endDate);
    Task AddCarAsync(Car? car);
    Task<bool> UpdateCarAsync(Car car);
    Task<bool> IsCarExistsAsync(Guid carId);
}
