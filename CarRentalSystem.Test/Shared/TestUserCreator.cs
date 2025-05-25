using CarRentalSystem.Api;
using CarRentalSystem.Db;
using CarRentalSystem.Db.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Test.Shared;

public abstract class TestUserCreator
{
    public static async Task CreateTestUser(User user, WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalSystemDbContext>();
        
        var existingUser = await dbContext.Users.FindAsync(user.Id);
        if (existingUser is null)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
        }
    }
}