using AutoFixture;
using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Test.Fixtures;
using Microsoft.Extensions.Logging;

namespace CarRentalSystem.Test.Controllers;

using System.Net;
using System.Net.Http.Json;
using Xunit;


public class UserControllerTest(SqlServerFixture sqlServerFixture) : IClassFixture<SqlServerFixture>
{
    private readonly HttpClient _client = sqlServerFixture.Client;
    //private readonly SqlServerFixture _fixture = sqlServerFixture;
    private readonly IFixture _fixture = new Fixture();
    ILogger<UserControllerTest> logger = new Logger<UserControllerTest>(new LoggerFactory());
    [Fact]
    public async Task GetProducts_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/api/users");
 
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedResponse()
    {
        // Arrange
        var user = _fixture.Create<SignupRequestBodyDto>();
    
        // Act
        var response = await _client.PostAsJsonAsync("/api/authentication/sign-up", user);
    
        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    
        var createdProduct = await response.Content.ReadFromJsonAsync<User>();
        Assert.NotNull(createdProduct);
        Assert.Equal(user.Email, createdProduct.Email);
    }
}