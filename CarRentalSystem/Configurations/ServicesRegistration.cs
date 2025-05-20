using CarRentalSystem.Db.Contexts;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Configurations;

public static class ServicesRegistration
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddDbContext<CarRentalSystemDbContext>();
    }
}