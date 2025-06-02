using CarRentalSystem.Api.Mappers.Authentication;
using CarRentalSystem.Api.Mappers.Cars;
using CarRentalSystem.Api.Mappers.Reservations;
using CarRentalSystem.Api.Mappers.Users;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class AddMappers
{
    public static void RegisterMappers(this IServiceCollection services)
    {
        services.AddSingleton<SignupRequestMapper>();     
        services.AddSingleton<UserProfileMapper>();     
        services.AddSingleton<CarResponseMapper>();     
        services.AddSingleton<UserResponseMapper>();     
        services.AddSingleton<AddReservationMapper>();     
        services.AddSingleton<CarRequestMapper>();     
        services.AddSingleton<UpdateReservationMapper>();     
        services.AddSingleton<UpdateCarMapper>();     
    }
}
