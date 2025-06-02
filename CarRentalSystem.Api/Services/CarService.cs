using CarRentalSystem.Api.Mappers.Cars;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Sieve.Models;

namespace CarRentalSystem.Api.Services;

public class CarService(ICarRepository carRepository, UpdateCarMapper updateCarMapper) : ICarService
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

    public async Task AddCarAsync(Car? car)
    {
         await carRepository.AddCarAsync(car);
    }

    public async Task<bool> UpdateCarAsync(Guid carId, UpdateCarRequestDto updateCarRequestDto)
    {
        var car = await carRepository.GetCarById(carId);
        if (car is null)
        {
            return false;
        }

        updateCarMapper.MapUpdateCarBodyToCar(updateCarRequestDto, car);
       
        await carRepository.UpdateCarAsync(car);
        return true;
    }

    public async Task<bool> IsCarExistsAsync(Guid carId)
    {
        return await carRepository.IsCarExist(carId);
    }
}