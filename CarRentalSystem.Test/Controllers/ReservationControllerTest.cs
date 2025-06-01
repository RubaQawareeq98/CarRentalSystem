using AutoFixture;
using CarRentalSystem.Api.Models.Reservations;
using CarRentalSystem.Db.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using CarRentalSystem.Api;
using CarRentalSystem.Db.Enums;
using CarRentalSystem.Test.Fixtures;
using CarRentalSystem.Test.Handlers;
using CarRentalSystem.Test.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Test.Controllers;

public class ReservationControllerTest : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private const string BaseUrl = "/api/reservations";
    private readonly IFixture _fixture = new Fixture();

    public ReservationControllerTest(SqlServerFixture sqlServerFixture)
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
    public async Task AddReservation_ValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var car = _fixture.Build<Car>()
            .With(c => c.IsAvailable, true)
            .Without(c => c.Reservations)
            .Create();
        await CarTestUtilities.AddTestCars([car], _factory);
        var user = _fixture.Create<User>();
        await UserTestUtilities.CreateTestUser(user, _factory);

        var reservationDto = _fixture.Build<AddReservationBodyDto>()
            .With(r => r.CarId, car.Id)
            .With(r => r.StartDate, DateTime.Now.AddDays(1))
            .With(r => r.EndDate, DateTime.Now.AddDays(3))
            .With(r => r.UserId, user.Id)
            .Create();

        TestAuthenticationHeader.SetTestAuthHeader(_client, user.Id, UserRole.Customer);

        // Act
        var response = await _client.PostAsJsonAsync(BaseUrl, reservationDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task AddReservation_CarNotAvailable_ShouldReturnBadRequest()
    {
        // Arrange
        var car = _fixture.Build<Car>()
            .With(c => c.IsAvailable, false)
            .Without(c => c.Reservations)
            .Create();
        await CarTestUtilities.AddTestCars([car], _factory);

        var reservationDto = _fixture.Build<AddReservationBodyDto>()
            .With(r => r.CarId, car.Id)
            .Create();

        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Customer);

        // Act
        var response = await _client.PostAsJsonAsync(BaseUrl, reservationDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetUserReservations_ValidRequest_ShouldReturnReservations()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var car = _fixture.Create<Car>();
        var reservations = _fixture.Build<Reservation>()
            .With(r => r.UserId, user.Id)
            .With(r => r.CarId, car.Id)
            .CreateMany(3)
            .ToList();

        await CarTestUtilities.AddTestCars([car], _factory);
        await UserTestUtilities.CreateTestUser(user, _factory);
        await ReservationTestUtilities.AddTestReservations(reservations, _factory);
        TestAuthenticationHeader.SetTestAuthHeader(_client, user.Id, UserRole.Customer);

        // Act
        var response = await _client.GetAsync($"{BaseUrl}/user/{user.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseReservations = await response.Content.ReadFromJsonAsync<List<Reservation>>();
        responseReservations.Should().NotBeNull().And.HaveCount(3);
    }
    

    [Fact]
    public async Task UpdateReservation_ValidRequest_ShouldReturnNoContent()
    {
        // Arrange
        var car = _fixture.Create<Car>();
        var user = _fixture.Create<User>();
        
        var reservation = _fixture.Build<Reservation>()
            .With(r => r.CarId, car.Id)
            .With(r => r.UserId, user.Id)
            .Create();
        
        await CarTestUtilities.AddTestCars([car], _factory);
        await UserTestUtilities.CreateTestUser(user, _factory);
        await ReservationTestUtilities.AddTestReservations([reservation], _factory);

        var updateDto = _fixture.Build<UpdateReservationDto>()
            .With(r => r.CarId, car.Id)
            .Create();
        TestAuthenticationHeader.SetTestAuthHeader(_client, user.Id, UserRole.Customer);

        // Act
       var response = await _client.PutAsJsonAsync($"{BaseUrl}/{reservation.Id}", updateDto);

        // Assert
       response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task UpdateReservation_InvalidReservationId_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = _fixture.Create<Guid>();
        var updateDto = _fixture.Create<UpdateReservationDto>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Customer);

        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/reservation/{nonExistentId}", updateDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetAllReservations_AdminRole_ShouldReturnAllReservations()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var car = _fixture.Create<Car>();   
        
        var reservations = _fixture.Build<Reservation>()
            .With(r => r.CarId, car.Id)
            .With(r => r.UserId, user.Id)
            .CreateMany(5)
            .ToList();
        await ReservationTestUtilities.AddTestReservations(reservations, _factory);

        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Admin);

        // Act
        var response = await _client.GetAsync(BaseUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var responseReservations = await response.Content.ReadFromJsonAsync<List<ReservationResponseDto>>();
        responseReservations.Should().NotBeNull().And.HaveCount(5);
    }

    [Fact]
    public async Task GetAllReservations_NotAdminRole_ShouldReturnForbidden()
    {
        // Arrange
        TestAuthenticationHeader.SetTestAuthHeader(_client, _fixture.Create<Guid>(), UserRole.Customer);

        // Act
        var response = await _client.GetAsync(BaseUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}
