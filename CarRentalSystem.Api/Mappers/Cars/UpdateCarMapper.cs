using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Api.Mappers.Cars;

public class UpdateCarMapper
{
    public void MapUpdateCarBodyToCar(UpdateCarRequestDto updateCarRequestDto, Car car)
    {
        if (!string.IsNullOrWhiteSpace(updateCarRequestDto.Location))
        {
            car.Location = updateCarRequestDto.Location;
        }

        if (!string.IsNullOrWhiteSpace(updateCarRequestDto.Brand))
        {
            car.Brand = updateCarRequestDto.Brand;
        }

        if (!string.IsNullOrWhiteSpace(updateCarRequestDto.Model))
        {
            car.Model = updateCarRequestDto.Model;
        }

        if (updateCarRequestDto.Year.HasValue)
        {
            car.Year = updateCarRequestDto.Year.Value;
        }

        if (updateCarRequestDto.Price.HasValue)
        {
            car.Price = updateCarRequestDto.Price.Value;
        }
    }
}