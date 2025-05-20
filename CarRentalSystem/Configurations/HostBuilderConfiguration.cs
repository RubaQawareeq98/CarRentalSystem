using CarRentalSystem.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CarRentalSystem.Configurations;

public static class HostBuilderConfiguration
{
    public static IServiceProvider Configure(this IHostBuilder hostBuilder)
    {
        var host = hostBuilder
            .ConfigureAppConfiguration((_, config) => { config.AddJsonFile("./appsettings.json"); })
            .ConfigureServices((context, services) =>
            {
                var connectionString = context.Configuration.GetConnectionString("sqlConnectionString");
                services.AddDbContext<CarRentalSystemDbContext>(options => options.UseSqlServer(connectionString));

                services.RegisterServices();
            })
            .Build();
        var scope = host.Services.CreateScope();
        return scope.ServiceProvider;
    }
}