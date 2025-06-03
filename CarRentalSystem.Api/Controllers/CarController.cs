using CarRentalSystem.Api.Mappers.Cars;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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

    [HttpGet("{id}")]
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
    public async Task<ActionResult<List<CarResponseDto>>> GetSearchCars([FromQuery] CarSearchDto carSearchRequest)
    {
        var filteredCars = await carService.SearchCarsAsync(carSearchRequest);
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
    [HttpPatch("{carId}")]
    public async Task<ActionResult> UpdateCar(Guid carId, [FromBody] JsonPatchDocument<Car> patchDoc)
    {

        var car = await carService.GetCarByIdAsync(carId);
        if (car is null)
            return NotFound();

        patchDoc.ApplyTo(car, ModelState);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var isSuccess = await carService.UpdateCarAsync(car);

        return isSuccess ? Ok("Car updated successfully") : StatusCode(500, "Update failed");
    }

}