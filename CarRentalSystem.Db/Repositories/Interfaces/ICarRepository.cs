using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Db.Repositories.Interfaces;

public interface ICarRepository
{
    Task<List<Car>> GetCarsAsync();
    Task<List<Car>> GetAvailableCarsAsync();
    Task<List<Car>> GetFilteredCarsAsync(CarSearchDto carSearchDto);
}