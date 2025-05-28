using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Db.Models;
using Riok.Mapperly.Abstractions;

namespace CarRentalSystem.Api.Mappers.Users;

[Mapper]
public partial class UserResponseMapper
{
    public partial List<UserResponseDto> ToUserResponseDtos(List<User> users);
}