using CarRentalSystem.Api;
using CarRentalSystem.Db;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Test.Helpers;

public static class DatabaseCleaner
{
    public static async Task ClearDatabaseAsync( WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CarRentalSystemDbContext>();
        var tableNames = new[] { "Reservations", "Users", "Cars" };

        foreach (var table in tableNames)
        {
            await db.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}];");
        }
    }
}