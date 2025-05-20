using CarRentalSystem.Api.Mappers.Authentication;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class AddMappers
{
    public static void RegisterMappers(this IServiceCollection services)
    {
        services.AddSingleton<SignupRequestMapper>();     
        
    }
}
