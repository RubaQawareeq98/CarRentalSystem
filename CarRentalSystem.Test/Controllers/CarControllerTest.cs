using AutoFixture;
using CarRentalSystem.Api.Models.Cars;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Test.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using CarRentalSystem.Api;
using CarRentalSystem.Db.Enums;
using CarRentalSystem.Test.Handlers;
using CarRentalSystem.Test.Helpers;
using CarRentalSystem.Test.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Test.Controllers;

public class CarControllerTest : IClassFixture<SqlServerFixture>, IAsyncLifetime
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
    }

    [Fact]
    public async Task GetCars_ValidRequest_ShouldReturnsOkWithCars()
    {
        // Arrange
        var cars = _fixture.Build<Car>()
            .Without(c => c.Reservations)
            .With(c => c.IsAvailable, true)
            .CreateMany(5)
            .ToList();
        
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Customer);
        await CarTestUtilities.AddTestCars(cars, _factory);
        
        // Act
        var response = await _client.GetAsync($"{BaseUrl}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseCars = await response.Content.ReadFromJsonAsync<List<CarResponseDto>>();
        responseCars.Should().NotBeNull().And.HaveCount(5);
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
        
        await CarTestUtilities.AddTestCars(availableCars.Concat(unavailableCars).ToList(), _factory);
        
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
        var searchDto = _fixture.Build<CarSearchDto>()
            .With(s => s.StartDate, DateTime.Today)
            .With(s => s.EndDate, DateTime.Today.AddDays(3))
            .With(s => s.MinPrice, 10000)
            .With(s => s.MaxPrice, 20000)
            .With(s => s.MinYear, 2020)
            .With(s => s.MaxYear, 2023) 
            .Create();
        
        var redCars = _fixture.Build<Car>()
            .With(c => c.Color, searchDto.Color)
            .With(c => c.Brand, searchDto.Brand)
            .With(c => c.Model, searchDto.Model)
            .With(c => c.Location, searchDto.Location)
            .With(c => c.Price, searchDto.MinPrice)
            .With(c => c.Year, searchDto.MinYear)
            .With(c => c.IsAvailable, true)
            .Without(c => c.Reservations) 
            .CreateMany(2)
            .ToList();

        var cars = _fixture.Build<Car>()
            .With(c => c.IsAvailable, true)
            .Without(c => c.Reservations)
            .CreateMany(3)
            .ToList();

        
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Customer);

        await CarTestUtilities.AddTestCars(redCars.Concat(cars).ToList(), _factory);

        // Act
        var query = $"""
            ?filters=Color=={searchDto.Color}&Brand=={searchDto.Brand}&Model=={searchDto.Model}&Location=={searchDto.Location}&
            Year>={searchDto.MinYear}&Year<={searchDto.MaxYear}&Price>={searchDto.MinPrice}&Price<={searchDto.MaxPrice}
            """;

        var response = await _client.GetAsync($"{BaseUrl}/search{query}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseCars = await response.Content.ReadFromJsonAsync<List<CarResponseDto>>();
        responseCars.Should().NotBeNull().And.HaveCount(2);
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
        response.StatusCode.Should().Be(HttpStatusCode.Created);
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
        
        await CarTestUtilities.AddTestCars([car], _factory);

        var updateRequest = _fixture.Create<UpdateCarRequestDto>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);

        // Act
        var response = await _client.PatchAsJsonAsync($"{BaseUrl}/{car.Id}", updateRequest);

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
        var response = await _client.PatchAsJsonAsync($"{BaseUrl}/{carId}", updateRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await DatabaseCleaner.ClearDatabaseAsync(_factory);
    }
}