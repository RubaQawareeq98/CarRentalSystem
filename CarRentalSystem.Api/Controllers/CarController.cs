using CarRentalSystem.Api.Mappers.Cars;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;

namespace CarRentalSystem.Api.Controllers;

[Authorize]
[Route("api/cars")]
[ApiController]
public class CarController(
    ICarService carService,
    CarResponseMapper mapper,
    CarRequestMapper carRequestMapper) : ControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<CarResponseDto>>> GetCars([FromQuery] SieveModel sieveModel)
    {
        var cars = await carService.GetAllCarsAsync(sieveModel);
        var carsResponse = mapper.ToCarResponseDto(cars);
        
        return Ok(carsResponse);
    }

    [HttpGet("car/{id}")]
    public async Task<ActionResult<CarResponseDto>> GetCar(Guid id)
    {
        var car = await carService.GetCarByIdAsync(id);
        if (car is null)
        {
            return NotFound();
        }
        
        return Ok(car);
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<CarResponseDto>>> GetAvailableCars([FromQuery] SieveModel sieveModel)
    {
        var cars = await carService.GetAvailableCarsAsync(sieveModel);
        var carsResponse = mapper.ToCarResponseDto(cars);
        
        return Ok(carsResponse);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<CarResponseDto>>> GetSearchCars([FromQuery] CarSearchDto carSearchDto)
    {
        var filteredCars = await carService.SearchCarsAsync(carSearchDto);
        var carsResponse = mapper.ToCarResponseDto(filteredCars);
    
        return Ok(carsResponse);
    }

    
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult> AddCar(CarRequestDto carRequestDto)
    {
        var car = carRequestMapper.ToCar(carRequestDto);
        await carService.AddCarAsync(car);
        
        return CreatedAtAction(
            nameof(GetCar),
            new { id = car.Id },
            car
        );
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{carId}")]
    public async Task<ActionResult> UpdateCar(Guid carId, CarRequestDto carRequestDto)
    {
        var car = carRequestMapper.ToCar(carRequestDto);
        var isSuccess = await carService.UpdateCarAsync(car);
        
        return isSuccess? Ok("Car updated successfully") : NotFound();
    }
}