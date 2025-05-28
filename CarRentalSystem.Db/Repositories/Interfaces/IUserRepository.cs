using CarRentalSystem.Db.Models;

namespace CarRentalSystem.Db.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> FindUserByEmailAsync(string email);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<bool> IsEntityExist(Guid id);
}
