using CarRentalSystem.Api.Services;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db;
using CarRentalSystem.Db.Repositories;
using CarRentalSystem.Db.Repositories.Interfaces;

namespace CarRentalSystem.Api.ServiceRegistration;

public static class AddServices
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddDbContext<CarRentalSystemDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        // services.AddScoped<IReservationRepository, ReservationRepository>();
        // services.AddScoped<IOrderRepository, OrderRepository>();
        // services.AddScoped<IOrderItemRepository, OrderItemRepository>();
        // services.AddScoped<IMenuItemRepository, MenuItemRepository>();
        // services.AddScoped<IRestaurantRepository, RestaurantRepository>();
        // services.AddScoped<ITableRepository, TableRepository>();
        services.AddScoped<IJwtTokenGeneratorService, JwtTokenGeneratorService>();
        // services.AddScoped<IUserRepository, UserRepository>();
    }
}