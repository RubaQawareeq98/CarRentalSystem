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
using CarRentalSystem.Test.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace CarRentalSystem.Test.Controllers;

public class UserControllerTests : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;
    private const string BaseUrl = "/api/users";
    private readonly IFixture _fixture = new Fixture();

    public UserControllerTests(SqlServerFixture fixture)
    {
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        
        _factory = fixture.Factory;
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
        AuthenticationHeader.SetTestAuthHeader(_client, user.Id, UserRole.Customer);
        
        await UserRepo.CreateTestUser(user, _factory);

        // Act
        var response = await _client.GetAsync($"{BaseUrl}/profile/{user.Id}");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<ProfileResponseDto>();
        Assert.NotNull(responseContent);
        Assert.Equal(user.FirstName, responseContent.FirstName);
        Assert.Equal(user.LastName, responseContent.LastName);
        Assert.Equal(user.Email, responseContent.Email);
    }

    [Fact]
    public async Task GetUserProfile_InvalidUserId_ShouldReturnNotFound()
    {
        // Arrange
        var user = _fixture.Create<User>();
        AuthenticationHeader.SetTestAuthHeader(_client, user.Id, UserRole.Customer);
        
        // Act
        var response = await _client.GetAsync($"{BaseUrl}/profile/{user.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProfile_InValidUserId_ShouldReturnsNotFound()
    {
        // Arrange
        var userDto = _fixture.Create<UpdateProfileBodyDto>();
        var userId = _fixture.Create<Guid>(); 
        AuthenticationHeader.SetTestAuthHeader(_client, userId, UserRole.Customer);
        
        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/profile/{userId}", userDto);
        
        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Fact]
    public async Task UpdateProfile_ValidRequest_ShouldUpdateUser()
    {
        // Arrange
        var user = _fixture.Build<User>()
            .Without(u => u.Reservations)
            .Create();
        
        await UserRepo.CreateTestUser(user, _factory);
        
        var updateDto = _fixture.Build<UpdateProfileBodyDto>()
            .With(x => x.Country, "Palestine")
            .Create();
        AuthenticationHeader.SetTestAuthHeader(_client, user.Id, UserRole.Customer);
        
        // Act
        var response = await _client.PutAsJsonAsync($"{BaseUrl}/profile/{user.Id}", updateDto);

        // Assert
        response.EnsureSuccessStatusCode();
        
        var getResponse = await _client.GetAsync($"{BaseUrl}/profile/{user.Id}");
        var updatedUser = await getResponse.Content.ReadFromJsonAsync<ProfileResponseDto>();
        
        Assert.NotNull(updatedUser);
        Assert.Equal(updateDto.FirstName, updatedUser.FirstName);
        Assert.Equal(updateDto.LastName, updatedUser.LastName);
        Assert.Equal(updateDto.Country, updatedUser.Country);
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
            await UserRepo.CreateTestUser(user, _factory);
        }
        var userId = _fixture.Create<Guid>();
        AuthenticationHeader.SetTestAuthHeader(_client, userId, UserRole.Admin);

        
        // Act
        var response = await _client.GetAsync(BaseUrl);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadFromJsonAsync<List<UserResponseDto>>();
        Assert.NotNull(responseContent);
        Assert.True(responseContent.Count >= users.Count); 
    }

    [Fact]
    public async Task GetAllUsers_NotAdminRole_ShouldReturnForbidden()
    {
        // Arrange
        var userId = _fixture.Create<Guid>();
        AuthenticationHeader.SetTestAuthHeader(_client, userId, UserRole.Customer);

        // Act
        var response = await _client.GetAsync(BaseUrl);

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}