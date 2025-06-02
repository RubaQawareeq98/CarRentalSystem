using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Db.Models;
using Riok.Mapperly.Abstractions;

namespace CarRentalSystem.Api.Mappers.Cars;

[Mapper]
public partial class CarRequestMapper
{
    public partial Car? ToCar(CarRequestDto carRequestDto);
}