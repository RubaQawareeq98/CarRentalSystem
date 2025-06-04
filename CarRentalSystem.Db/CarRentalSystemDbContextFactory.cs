using Microsoft.Extensions.Configuration;

namespace CarRentalSystem.Db;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class CarRentalSystemDbContextFactory : IDesignTimeDbContextFactory<CarRentalSystemDbContext>
{
    public CarRentalSystemDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../CarRentalSystem.API");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json")
            .Build();
        var optionsBuilder = new DbContextOptionsBuilder<CarRentalSystemDbContext>();
        var connectionString = configuration.GetConnectionString("sqlConnectionString");

        optionsBuilder.UseSqlServer(connectionString);

        return new CarRentalSystemDbContext(optionsBuilder.Options);
    }
}
