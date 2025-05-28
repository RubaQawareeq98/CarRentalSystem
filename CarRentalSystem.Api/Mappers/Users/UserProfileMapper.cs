using CarRentalSystem.Api.Models.Profile;
using CarRentalSystem.Db.Models;
using Riok.Mapperly.Abstractions;

namespace CarRentalSystem.Api.Mappers.Users;

[Mapper]
public partial class UserProfileMapper
{
    public partial ProfileResponseDto ToProfileResponseDto(User user);
    public partial User UpdateUser(UpdateProfileBodyDto source);
}