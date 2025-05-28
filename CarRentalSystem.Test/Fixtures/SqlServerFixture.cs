using CarRentalSystem.Api;
using CarRentalSystem.Db;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.MsSql;
using Program = CarRentalSystem.Api.Program;

namespace CarRentalSystem.Test.Fixtures;

public class SqlServerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer;
    private IDbContextTransaction _transaction;

    public WebApplicationFactory<Program> Factory { get; private set; }

    public HttpClient Client { get; private set; }
    private string ConnectionString { get; set; } = string.Empty;

    public SqlServerFixture()
    {
        IConfiguration configuration =
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("testAppsettings.json", optional: false)
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

        Factory = new WebApplicationFactory<Program>() 
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices( void (services) =>
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
                });
            });

        Client = Factory.CreateClient();
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CarRentalSystemDbContext>();
        await db.Database.EnsureCreatedAsync();
        _transaction = await db.Database.BeginTransactionAsync();
    }

    public async Task DisposeAsync()
    {
        try
        {
            await _transaction.RollbackAsync();
        }
        finally
        {
            Client.Dispose();
            await Factory.DisposeAsync();
            await _dbContainer.DisposeAsync();
        }
    }
    
    public async Task ClearDatabaseAsync()
    {
        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CarRentalSystemDbContext>();

        var tableNames = new[] { "Reservations", "Users", "Cars" };

        foreach (var table in tableNames)
        {
            await db.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}];");
        }
    }
}