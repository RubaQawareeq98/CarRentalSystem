using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Db.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> FindUserByEmailAsync(string email);
    Task<User?> FindUserByIdAsync(Guid userId);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<bool> IsEntityExist(Guid id);
}
