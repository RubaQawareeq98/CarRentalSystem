using CarRentalSystem.Api.Mappers.Authentication;
using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Api.Models.Profile;
using CarRentalSystem.Db.Models;
using Sieve.Models;

namespace CarRentalSystem.Api.Services.Interfaces;

public interface IUserService
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> AuthenticateUserAsync(string email, string password);
    Task RegisterUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<List<User>> GetAllUsersAsync(SieveModel sieveModel);
    Task<bool> UserExistsAsync(Guid id);
    Task<(bool Success, string Message)> SignupAsync(SignupRequestBodyDto request);
}