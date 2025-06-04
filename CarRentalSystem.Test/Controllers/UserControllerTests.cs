using System.Net;
using CarRentalSystem.Api.Models.Profile;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Test.Fixtures;
using System.Net.Http.Json;
using AutoFixture;
using CarRentalSystem.Api;
using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Db.Enums;
using CarRentalSystem.Test.Handlers;
using CarRentalSystem.Test.Helpers;
using CarRentalSystem.Test.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Test.Controllers;

public class UserControllerTests : IClassFixture<SqlServerFixture>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private const string BaseUrl = "/api/users";
    private readonly IFixture _fixture = new Fixture();

    public UserControllerTests(SqlServerFixture sqlServerFixture)
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
    public async Task GetUserProfile_ValidUserId_ShouldReturnsUserProfile()
    {
        // Arrange
        var user = _fixture.Create<User>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, user.Id, UserRole.Customer);
        
        await UserTestUtilities.CreateTestUser(user, _factory);

        // Act
        var response = await _client.GetAsync($"{BaseUrl}/profile/{user.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<ProfileResponseDto>();

        responseContent.Should().NotBeNull();
        responseContent.FirstName.Should().Be(user.FirstName);
        responseContent.LastName.Should().Be(user.LastName);
        responseContent.Email.Should().Be(user.Email);
    }

    [Fact]
    public async Task GetUserProfile_InvalidUserId_ShouldReturnNotFound()
    {
        // Arrange
        var user = _fixture.Create<User>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, user.Id, UserRole.Customer);
        
        // Act
        var response = await _client.GetAsync($"{BaseUrl}/profile/{user.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProfile_InValidUserId_ShouldReturnsNotFound()
    {
        // Arrange
        var userDto = _fixture.Create<UpdateProfileBodyDto>();
        var userId = _fixture.Create<Guid>(); 
        TestAuthenticationHeader.SetTestAuthHeader(_client, userId, UserRole.Customer);
        
        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/profile/{userId}", userDto);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task UpdateProfile_ValidRequest_ShouldUpdateUser()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .Without(u => u.Reservations)
            .Create();
        const string updatedCountry = "Palestine";
        
        await UserTestUtilities.CreateTestUser(user, _factory);
        
        var updateDto = _fixture.Build<UpdateProfileBodyDto>()
            .With(x => x.Country, updatedCountry)
            .Create();
        
        TestAuthenticationHeader.SetTestAuthHeader(_client, user.Id, UserRole.Customer);
        
        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/profile/{user.Id}", updateDto);

        // Assert
        response.EnsureSuccessStatusCode();
        
        var getResponse = await _client.GetAsync($"{BaseUrl}/profile/{user.Id}");
        var updatedUser = await getResponse.Content.ReadFromJsonAsync<ProfileResponseDto>();
        
        updatedUser.Should().NotBeNull();
        updatedUser.FirstName.Should().Be(updateDto.FirstName);
        updatedUser.LastName.Should().Be(updateDto.LastName);
        updatedUser.Country.Should().Be(updatedCountry);
    }
    
    [Fact]
    public async Task GetAllUsers_AsAdmin_ShouldReturnAllUsers()
    {
        // Arrange
        var users = _fixture.Build<User>()
            .Without(u => u.Reservations)
            .CreateMany(3).ToList();
        
        foreach (var user in users)
        {
            await UserTestUtilities.CreateTestUser(user, _factory);
        }
        var userId = _fixture.Create<Guid>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, userId, UserRole.Admin);

        
        // Act
        var response = await _client.GetAsync(BaseUrl);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<List<UserResponseDto>>();
        responseContent.Should().NotBeNull();
        responseContent.Should().HaveCount(users.Count);
    }

    [Fact]
    public async Task GetAllUsers_NotAdminRole_ShouldReturnForbidden()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        TestAuthenticationHeader.SetTestAuthHeader(_client, userId, UserRole.Customer);

        // Act
        var response = await _client.GetAsync(BaseUrl);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await DatabaseCleaner.ClearDatabaseAsync(_factory);
    }
}