using CarRentalSystem.Api.Mappers.Authentication;
using CarRentalSystem.Api.Mappers.Users;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class AddMappers
{
    public static void RegisterMappers(this IServiceCollection services)
    {
        services.AddSingleton<SignupRequestMapper>();     
        services.AddSingleton<UserProfileMapper>();     
        
    }
}
