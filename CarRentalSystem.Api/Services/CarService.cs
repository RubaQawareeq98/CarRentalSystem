using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Sieve.Models;

namespace CarRentalSystem.Api.Services;

public class CarService(ICarRepository carRepository) : ICarService
{
    public async Task<List<Car>> GetAllCarsAsync(SieveModel sieveModel)
    {
        return await carRepository.GetAvailableCarsAsync(sieveModel);
    }

    public async Task<List<Car>> GetAvailableCarsAsync(SieveModel sieveModel)
    {
        return await carRepository.GetAvailableCarsAsync(sieveModel);
    }

    public async Task<Car?> GetCarByIdAsync(Guid id)
    {
        var isCarExist = await IsCarExistsAsync(id);
        if (!isCarExist)
        {
            return null;
        }

        return await carRepository.GetCarById(id);
    }

    public async Task<List<Car>> SearchCarsAsync(CarSearchDto carSearchDto)
    {
        return await carRepository.GetFilteredCarsAsync(carSearchDto);
    }

    public async Task<bool> IsCarAvailableAsync(Guid carId, DateTime startDate, DateTime endDate)
    {
        return await carRepository.IsCarAvailable(carId, startDate, endDate);
    }

    public async Task AddCarAsync(Car car)
    {
         await carRepository.AddCarAsync(car);
    }

    public async Task<bool> UpdateCarAsync(Car car)
    {
        var isCarExist = await carRepository.IsCarExist(car.Id);
        if (!isCarExist)
        {
            return false;
        }
        await carRepository.UpdateCarAsync(car);
        return true;
    }

    public async Task<bool> IsCarExistsAsync(Guid carId)
    {
        return await carRepository.IsCarExist(carId);
    }
}