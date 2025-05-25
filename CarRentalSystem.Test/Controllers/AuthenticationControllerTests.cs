using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using CarRentalSystem.Api;
using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Test.Fixtures;
using CarRentalSystem.Test.Shared;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CarRentalSystem.Test.Controllers;

public class AuthenticationControllerTests : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _client;
    private readonly IFixture _fixture = new Fixture();
    private const string BaseUrl = "/api/authentication";
    private readonly WebApplicationFactory<Program> _factory;


    public AuthenticationControllerTests(SqlServerFixture sqlServerFixture)
    {
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        _client = sqlServerFixture.Client;
        _factory = sqlServerFixture.Factory;
    }

    [Fact]
    public async Task SignUp_ValidRequestBody_ShouldReturnsOk()
    {
        // Arrange
        var password = _fixture.Create<string>();
        var user = _fixture.Build<SignupRequestBodyDto>()
            .With(x => x.Password, password)
            .With(x => x.ConfirmPassword, password)
            .Create();

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/sign-up", user);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task SignUp_PasswordsDoNotMatch_ShouldReturnsBadRequest()
    {
        // Arrange
        var password = _fixture.Create<string>();
        var confirmPassword = _fixture.Create<string>();
        
        var user = _fixture.Build<SignupRequestBodyDto>()
            .With(x => x.Password, password)
            .With(x => x.ConfirmPassword, confirmPassword)
            .Create();

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/sign-up", user);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SignUp_EmailAlreadyExists_ShouldReturnsBadRequest()
    {
        // Arrange
        var password = _fixture.Create<string>();
        var email = _fixture.Create<string>();
        
        var user = _fixture.Build<SignupRequestBodyDto>()
            .With(x => x.Password, password)
            .With(x => x.ConfirmPassword, password)
            .With(x => x.Email, email)
            .Create();

        // Act
        await _client.PostAsJsonAsync($"{BaseUrl}/sign-up", user);

        var response = await _client.PostAsJsonAsync($"{BaseUrl}/sign-up", user);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task SignIn_ValidCredentials_ShouldReturnsOkWithToken()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        var loginRequest = new LoginRequestBodyDto { Email = email, Password = password };

        
        var user = _fixture.Build<User>()
            .With(x => x.Email, email)
            .With(x => x.Password, password)
            .Create();

        await UserRepo.CreateTestUser(user, _factory);
  

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/sign-in", loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        authResponse.Should().NotBeNull();
        authResponse.Token.Should().NotBeNull();
    }

    [Fact]
    public async Task SignIn_InvalidCredentials_ShouldReturnsUnauthorized()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        var loginRequest = new LoginRequestBodyDto { Email = email, Password = password };

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/sign-in", loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}