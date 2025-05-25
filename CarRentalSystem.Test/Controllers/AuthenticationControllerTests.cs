using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Test.Fixtures;

namespace CarRentalSystem.Test.Controllers;

public class AuthenticationControllerTests(SqlServerFixture sqlServerFixture) : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _client = sqlServerFixture.Client;
    private readonly IFixture _fixture = new Fixture();
    private const string BaseUrl = "/api/authentication";

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
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
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
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
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
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task SignIn_ValidCredentials_ShouldReturnsOkWithToken()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var password = _fixture.Create<string>();
        var loginRequest = new LoginRequestBodyDto { Email = email, Password = password };

        
        var user = _fixture.Build<SignupRequestBodyDto>()
            .With(x => x.Email, email)
            .With(x => x.Password, password)
            .With(x => x.ConfirmPassword, password)
            .Create();

        await _client.PostAsJsonAsync($"{BaseUrl}/sign-up", user);

        // Act
        var response = await _client.PostAsJsonAsync($"{BaseUrl}/sign-in", loginRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        Assert.NotNull(authResponse?.Token);
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

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}