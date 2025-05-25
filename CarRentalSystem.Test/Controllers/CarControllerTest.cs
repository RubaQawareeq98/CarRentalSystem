using AutoFixture;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Test.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using CarRentalSystem.Api;
using CarRentalSystem.Db;
using CarRentalSystem.Db.Enums;
using CarRentalSystem.Test.Handlers;
using CarRentalSystem.Test.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Test.Controllers;

public class CarControllerTest : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _client;
    private readonly IFixture _fixture = new Fixture();
    private const string BaseUrl = "/api/cars";
    private readonly WebApplicationFactory<Program> _factory;

    public CarControllerTest(SqlServerFixture sqlServerFixture)
    {
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _factory = sqlServerFixture.Factory;
        
        _client = _factory.WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        services.AddAuthentication("TestAuthScheme")
                            .AddScheme<AuthenticationSchemeOptions, AuthenticationHandlerTest>(
                                "TestAuthScheme", _ => { });
                    });
                }).CreateClient();
        
        sqlServerFixture.ClearDatabaseAsync().Wait();
    }

    [Fact]
    public async Task GetCars_ValidRequest_ShouldReturnsOkWithCars()
    {
        // Arrange
        var cars = _fixture.Build<Car>()
            .Without(c => c.Reservations)
            .CreateMany(5)
            .ToList();
        
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Customer);
       await AddTestCars(cars);
        

        // Act
        var response = await _client.GetAsync($"{BaseUrl}?pageNumber=1&pageSize=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseCars = await response.Content.ReadFromJsonAsync<List<CarResponseDto>>();
        responseCars.Should().NotBeNull().And.HaveCount(5);
    }

    [Theory]
    [InlineData(1, 0)]
    [InlineData(-1, 5)]
    public async Task GetCars_InvalidPagination_ShouldReturnsBadRequest(int pageNumber, int pageSize)
    {
        // Arrange
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Customer);

        // Act
        var response = await _client.GetAsync($"{BaseUrl}?pageNumber={pageNumber}&pageSize={pageSize}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAvailableCars_ValidRequest_ShouldReturnsOnlyAvailableCars()
    {
        // Arrange
        var availableCars = _fixture.Build<Car>()
            .With(c => c.IsAvailable, true)
            .Without(c => c.Reservations)
            .CreateMany(3)
            .ToList();

        var unavailableCars = _fixture.Build<Car>()
            .With(c => c.IsAvailable, false)
            .Without(c => c.Reservations)
            .CreateMany(2)
            .ToList();

        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Customer);
        await AddTestCars(availableCars.Concat(unavailableCars).ToList());

        // Act
        var response = await _client.GetAsync($"{BaseUrl}/available?pageNumber=1&pageSize=5");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseCars = await response.Content.ReadFromJsonAsync<List<CarResponseDto>>();
        responseCars.Should().NotBeNull().And.HaveCount(3);
    }

    [Fact]
    public async Task GetSearchCars_WithFilters_ShouldReturnsFilteredCars()
    {
        // Arrange
        var redCars = _fixture.Build<Car>()
            .With(c => c.Color, "Red")
            .With(c => c.Brand, "Toyota")
            .With(c => c.Year, 2020)
            .Without(c => c.Reservations)
            .CreateMany(2)
            .ToList();

        var cars = _fixture.Build<Car>()
            .Without(c => c.Reservations)
            .CreateMany(3)
            .ToList();
        
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Customer);

        await AddTestCars(redCars.Concat(cars).ToList());

        var searchDto = new CarSearchDto { Color = "Red", Brand = "Toyota", MinYear = 2019 };

        // Act
        var query = $"?Color={searchDto.Color}&Brand={searchDto.Brand}&MinYear={searchDto.MinYear}";
        var response = await _client.GetAsync($"{BaseUrl}/search{query}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseCars = await response.Content.ReadFromJsonAsync<List<CarResponseDto>>();
        responseCars.Should().NotBeNull().And.HaveCount(1);
    }

    [Fact]
    public async Task AddCar_AdminRole_ShouldReturnsSuccess()
    {
        // Arrange
        var carRequest = _fixture.Create<CarRequestDto>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);

        // Act
        var response = await _client.PostAsJsonAsync(BaseUrl, carRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task AddCar_NotAdminRole_ShouldReturnsForbidden()
    {
        // Arrange
        var carRequest = _fixture.Create<CarRequestDto>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Customer);

        // Act
        var response = await _client.PostAsJsonAsync(BaseUrl, carRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateCar_AdminRole_ShouldReturnsSuccess()
    {
        // Arrange
        var car = _fixture.Build<Car>()
            .Without(c => c.Reservations)
            .Create();
        
        await AddTestCars([car]);

        var updateRequest = _fixture.Create<CarRequestDto>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);

        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{car.Id}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateCar_InvalidCarId_ShouldReturnsNotFound()
    {
        // Arrange
        var carId = _fixture.Create<Guid>();
        var updateRequest = _fixture.Create<CarRequestDto>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);

        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/{carId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private async Task AddTestCars(List<Car> cars)
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CarRentalSystemDbContext>();
        
        dbContext.Cars.AddRange(cars);
        await dbContext.SaveChangesAsync();
    }
}