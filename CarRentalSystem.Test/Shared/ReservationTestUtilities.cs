using CarRentalSystem.Api;
using CarRentalSystem.Db;
using CarRentalSystem.Db.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Test.Shared;

public static class ReservationTestUtilities
{
    public static async Task AddTestReservations(List<Reservation> reservations, WebApplicationFactory<Program> factory)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalSystemDbContext>();
        dbContext.Reservations.AddRange(reservations);
        await dbContext.SaveChangesAsync();
    }
}