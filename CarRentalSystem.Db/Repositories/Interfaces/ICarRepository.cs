using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Db.Repositories.Interfaces;

public interface ICarRepository
{
    Task<List<Car>> GetCarsAsync(int pageNumber, int pageSize);
    Task<List<Car>> GetAvailableCarsAsync(int pageNumber, int pageSize);
    Task<List<Car>> GetFilteredCarsAsync(CarSearchDto carSearchDto);
    Task<bool> IsCarAvailable(Guid carId, DateTime startDate, DateTime endDate);
}