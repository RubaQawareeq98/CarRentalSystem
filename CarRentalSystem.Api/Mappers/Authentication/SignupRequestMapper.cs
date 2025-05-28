using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Db.Models;
using Riok.Mapperly.Abstractions;

namespace CarRentalSystem.Api.Mappers.Authentication;

[Mapper]
public partial class SignupRequestMapper
{
    public partial User ToUser(SignupRequestBodyDto signupRequestDto);
}
