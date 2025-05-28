using AutoFixture;
using AutoFixture.AutoMoq;
using CarRentalSystem.Api.Controllers;
using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Api.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarRentalSystem.Test.Controllers;

public class ResetPasswordControllerTest
{
    private readonly Mock<IResetPasswordService> _resetPasswordServiceMock;
    private readonly ResetPasswordController _controller;
    private readonly IFixture _fixture;

    public ResetPasswordControllerTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _resetPasswordServiceMock = _fixture.Freeze<Mock<IResetPasswordService>>();
        _controller = new ResetPasswordController(_resetPasswordServiceMock.Object);
    }

    [Fact]
    public async Task ForgotPassword_WithValidEmail_ReturnsOk()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var request = new ForgotPasswordDto { Email = email };
        const string message = "Password reset link has been sent";

        _resetPasswordServiceMock.Setup(x => x.ForgotPassword(email))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.ForgotPassword(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(message);

        _resetPasswordServiceMock.Verify(x => x.ForgotPassword(email), Times.Once);
    }

    [Fact]
    public async Task ForgotPassword_WithInvalidEmail_ReturnsNotFound()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var request = new ForgotPasswordDto { Email = email };
        const string message = "Email address not found";

        _resetPasswordServiceMock.Setup(x => x.ForgotPassword(email))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.ForgotPassword(request);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>()
            .Which.Value.Should().Be(message);

        _resetPasswordServiceMock.Verify(x => x.ForgotPassword(email), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_WithValidData_ReturnsOk()
    {
        // Arrange
        var dto = _fixture.Create<ResetPasswordDto>();
        const string message = "Password has been reset successfully.";

        _resetPasswordServiceMock.Setup(x => x.ResetPasswordAsync(dto))
            .ReturnsAsync((true, message));

        // Act
        var result = await _controller.ResetPassword(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(message);

        _resetPasswordServiceMock.Verify(x => x.ResetPasswordAsync(dto), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange
        var dto = _fixture.Create<ResetPasswordDto>();
        const string message = "Invalid or expired token.";

        _resetPasswordServiceMock.Setup(x => x.ResetPasswordAsync(dto))
            .ReturnsAsync((false, message));

        // Act
        var result = await _controller.ResetPassword(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(message);

        _resetPasswordServiceMock.Verify(x => x.ResetPasswordAsync(dto), Times.Once);
    }
}