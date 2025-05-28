
using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Api.Services.Interfaces;

public interface IJwtTokenGeneratorService
{
    Task<string> GenerateToken(User user);
}
