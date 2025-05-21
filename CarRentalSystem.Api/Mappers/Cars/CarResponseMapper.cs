using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Db.Models;
using Riok.Mapperly.Abstractions;

namespace CarRentalSystem.Api.Mappers.Cars;

[Mapper]
public partial class CarResponseMapper
{
    public partial List<CarResponseDto> ToCarResponseDto(List<Car> cars);
}