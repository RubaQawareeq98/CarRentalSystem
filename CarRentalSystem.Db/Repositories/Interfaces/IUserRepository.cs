using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Db.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> FindUserByEmailAsync(string email);
    Task<User?> FindUserByIdAsync(Guid userId);
    Task<User?> FindUserByCredentials(string? email, string? password);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<bool> IsEntityExist(Guid id);
}
