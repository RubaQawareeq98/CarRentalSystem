using CarRentalSystem.Api.Mappers.Cars;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarRentalSystem.Api.Controllers;

[Authorize]
[Route("api/cars")]
[ApiController]
public class CarController(ICarRepository carRepository,
    CarResponseMapper mapper,
    CarRequestMapper carRequestMapper) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<CarResponseDto>>> GetCars([FromQuery] SieveModel sieveModel)
    {
        var cars = await carRepository.GetCarsAsync(sieveModel);
        var carsResponse = mapper.ToCarResponseDto(cars);
        
        return Ok(carsResponse);
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<CarResponseDto>>> GetAvailableCars([FromQuery] SieveModel sieveModel)
    {
        var cars = await carRepository.GetAvailableCarsAsync(sieveModel);
        var carsResponse = mapper.ToCarResponseDto(cars);
        
        return Ok(carsResponse);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<CarResponseDto>>> GetSearchCars([FromQuery] CarSearchDto carSearchDto)
    {
        var filteredCars = await carRepository.GetFilteredCarsAsync(carSearchDto);
        var carsResponse = mapper.ToCarResponseDto(filteredCars);
    
        return Ok(carsResponse);
    }

    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> AddCar(CarRequestDto carRequestDto)
    {
        var car = carRequestMapper.ToCar(carRequestDto);
        await carRepository.AddCarAsync(car);
        
        return Ok("Car added successfully");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{carId}")]
    public async Task<ActionResult> UpdateCar(Guid carId, CarRequestDto carRequestDto)
    {
        var isCarExist = await carRepository.IsCarExist(carId);
        if (!isCarExist)
        {
            return NotFound("Car with given id does not exist");
        }
        var car = carRequestMapper.ToCar(carRequestDto);
        await carRepository.UpdateCarAsync(car);
        
        return Ok("Car updated successfully");
    }
}
