using CarRentalSystem.Db;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class AddContexts
{
    public static void RegisterContexts(this IServiceCollection services){
        services.AddDbContext<CarRentalSystemDbContext>();
    }
}