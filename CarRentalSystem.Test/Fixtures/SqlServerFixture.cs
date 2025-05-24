using CarRentalSystem.Api;
using CarRentalSystem.Db;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace CarRentalSystem.Test.Fixtures;

public class SqlServerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer;

    public HttpClient Client { get; private set; }
    public string ConnectionString { get; private set; }

    public SqlServerFixture()
    {
        IConfiguration configuration =
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var image = configuration["SqlServerContainer:Image"];
        var password = configuration["SqlServerContainer:Password"];

        _dbContainer = new MsSqlBuilder()
            .WithImage(image)
            .WithPassword(password)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        ConnectionString = _dbContainer.GetConnectionString();

        var factory = new WebApplicationFactory<Program>() 
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<CarRentalSystemDbContext>)); 

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<CarRentalSystemDbContext>(options =>
                    {
                        options.UseSqlServer(ConnectionString);
                    });

                    var sp = services.BuildServiceProvider();

                    using var scope = sp.CreateScope();
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CarRentalSystemDbContext>();

                    db.Database.EnsureCreated();
                });
            });

        Client = factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        Client.Dispose();
    }
}