using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CarRentalSystem.Db.Repositories;

public class UserRepository (CarRentalSystemDbContext context) : IUserRepository
{
    public async Task<User?> FindUserByEmailAsync(string email)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

    }

    public async Task<User?> FindUserByIdAsync(Guid userId)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
    
    public async Task AddUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<bool> IsEntityExist(Guid id)
    {
        return await context.Users.AnyAsync(u => u.Id == id);
    }
}
