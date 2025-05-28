using CarRentalSystem.Api.Models.Reservations;
using CarRentalSystem.Db.Models;
using Riok.Mapperly.Abstractions;

namespace CarRentalSystem.Api.Mappers.Reservations;

[Mapper]
public partial class UpdateReservationMapper
{
    public partial void UpdateReservation(UpdateReservationDto dto, Reservation reservation);
}

