using CarRentalSystem.Api.Models.Reservations;
using CarRentalSystem.Db.Models;
using Riok.Mapperly.Abstractions;

namespace CarRentalSystem.Api.Mappers.Reservations;

[Mapper]
public partial class AddReservationMapper
{
    public partial Reservation? ToReservation(AddReservationBodyDto addReservationDto);
    public partial List<ReservationResponseDto> ToReservationResponseDtos(List<Reservation?> reservations);
}