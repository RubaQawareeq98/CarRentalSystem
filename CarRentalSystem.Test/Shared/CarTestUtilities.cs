using CarRentalSystem.Api;
using CarRentalSystem.Db;
using CarRentalSystem.Db.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Test.Shared;

public static class CarTestUtilities
{
    public static async Task AddTestCars(List<Car> cars, WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalSystemDbContext>();
        
        dbContext.Cars.AddRange(cars);
        await dbContext.SaveChangesAsync();
    }
}