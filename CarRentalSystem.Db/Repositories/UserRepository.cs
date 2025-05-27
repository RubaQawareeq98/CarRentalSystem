using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CarRentalSystem.Db.Repositories;

public class UserRepository (CarRentalSystemDbContext context, ILogger<UserRepository>logger) : IUserRepository
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

    public async Task<User?> FindUserByCredentials(string? email, string? password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return null;
        }

        var user = await FindUserByEmailAsync(email);
        if (user is null)
        {
            return null;
        }

        //var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
        var isPasswordValid = true;
        return isPasswordValid ? user : null;
    }


    public async Task AddUserAsync(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
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
