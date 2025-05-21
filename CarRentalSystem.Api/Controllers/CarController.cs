using CarRentalSystem.Api.Mappers.Cars;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalSystem.Api.Controllers;

[Route("api/cars")]
[ApiController]
public class CarController(ICarRepository carRepository, CarResponseMapper mapper) : ControllerBase
{
    private int _maxPageSize = 80;

    [HttpGet]
    public async Task<ActionResult<List<CarResponseDto>>> GetCars(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be greater than 0");
        }
        _maxPageSize = Math.Min(_maxPageSize, pageSize);
        
        var cars = await carRepository.GetCarsAsync(pageNumber, pageSize);
        var carsResponse = mapper.ToCarResponseDto(cars);
        
        return Ok(carsResponse);
    }

    [HttpGet("available")]
    public async Task<ActionResult<List<CarResponseDto>>> GetAvailableCars(int pageNumber = 1, int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be greater than 0");
        }
        _maxPageSize = Math.Min(_maxPageSize, pageSize);
        
        var cars = await carRepository.GetAvailableCarsAsync(pageNumber, pageSize);
        var carsResponse = mapper.ToCarResponseDto(cars);
        
        return Ok(carsResponse);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<CarResponseDto>>> GetSearchCars(CarSearchDto carSearchDto)
    {
        var filteredCars = await carRepository.GetFilteredCarsAsync(carSearchDto);
        var carsResponse = mapper.ToCarResponseDto(filteredCars);
        
        return Ok(carsResponse);
    }
}
