using AutoFixture;
using AutoFixture.AutoMoq;
using CarRentalSystem.Api.Controllers;
using CarRentalSystem.Api.Models.Users;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CarRentalSystem.Test.Controllers;

public class ResetPasswordControllerTest
{
    private readonly Mock<IEmailService> _emailServiceMock ;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IResetTokenRepository> _tokenRepositoryMock;
    private readonly ResetPasswordController _controller;
    private readonly IFixture _fixture;

    public ResetPasswordControllerTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        _emailServiceMock = _fixture.Freeze<Mock<IEmailService>>();
        _userRepositoryMock = _fixture.Freeze<Mock<IUserRepository>>();
        _tokenRepositoryMock = _fixture.Freeze<Mock<IResetTokenRepository>>();
        
        _controller = new ResetPasswordController(
            _emailServiceMock.Object,
            _userRepositoryMock.Object,
            _tokenRepositoryMock.Object);
        
    }

    [Fact]
    public async Task ForgotPassword_WithValidEmail_SendsResetEmail()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var testUser = new User { Email = email };
        var request = new ForgotPasswordDto { Email = email };
        const string message = "Password reset link has been sent";
    
        _userRepositoryMock.Setup(x => x.FindUserByEmailAsync(email))
            .ReturnsAsync(testUser);
    
        _emailServiceMock.Setup(x => x.SendResetPasswordEmail(testUser))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ForgotPassword(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(message);
    
        _emailServiceMock.Verify(x => x.SendResetPasswordEmail(testUser), Times.Once);
        _userRepositoryMock.Verify(x => x.FindUserByEmailAsync(email), Times.Once);
    }
    
    [Fact]
    public async Task ForgotPassword_WithNonExistentEmail_ReturnsNotFound()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var request = new ForgotPasswordDto { Email = email };
    
        _userRepositoryMock.Setup(x => x.FindUserByEmailAsync(email))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.ForgotPassword(request);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        _emailServiceMock.Verify(x => x.SendResetPasswordEmail(It.IsAny<User>()), Times.Never);
    }
    
    [Fact]
    public async Task ResetPassword_WithValidToken_UpdatesPassword()
    {
        // Arrange
        const string message = "Password has been reset successfully.";
        var testEmail = _fixture.Create<string>();
        var testToken = _fixture.Create<string>();
        var newPassword = _fixture.Create<string>();
        var testUser = _fixture.Build<User>()
            .With(u => u.Email, testEmail)
            .Create();
        
        var token = _fixture.Build<PasswordResetToken>()
                .With(t => t.Email, testEmail)
                .With(t => t.Token, testToken)
                .With(t => t.ExpiryDate, DateTime.UtcNow.AddHours(1)) 
                .Create();
       
        var request = new ResetPasswordDto
        {
            Email = testEmail,
            Token = testToken,
            Password = newPassword,
            ConfirmPassword = newPassword
        };
    
        _tokenRepositoryMock.Setup(x => x.GetResetTokenAsync(testEmail, testToken))
            .ReturnsAsync(token);
    
        _userRepositoryMock.Setup(x => x.FindUserByEmailAsync(testEmail))
            .ReturnsAsync(testUser);
    
        _userRepositoryMock.Setup(x => x.UpdateUserAsync(testUser))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.ResetPassword(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(message);
    
        testUser.Password.Should().Be(newPassword);
        _userRepositoryMock.Verify(x => x.UpdateUserAsync(testUser), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_WithInvalidToken_ReturnsBadRequest()
    {
        // Arrange
        const string message = "Invalid or expired token.";
        var testEmail = _fixture.Create<string>();
        var invalidToken = _fixture.Create<string>();
        var newPassword = _fixture.Create<string>();
      
        var request = new ResetPasswordDto
        {
            Email = testEmail,
            Token = invalidToken,
            Password = newPassword,
            ConfirmPassword = newPassword
        };
    
        _tokenRepositoryMock.Setup(x => x.GetResetTokenAsync(testEmail, invalidToken))
            .ReturnsAsync((PasswordResetToken?)null);

        // Act
        var result = await _controller.ResetPassword(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(message);
    
        _userRepositoryMock.Verify(x => x.UpdateUserAsync(It.IsAny<User>()), Times.Never);

    }
    
    [Fact]
    public async Task ResetPassword_WithPasswordMismatch_ReturnsBadRequest()
    {
        // Arrange
        const string message = "Passwords do not match.";
        var testEmail = _fixture.Create<string>();
        var testToken = _fixture.Create<string>();
        var newPassword = _fixture.Create<string>();
        var confirmPassword = _fixture.Create<string>(); 
    
        var testUser = _fixture.Build<User>()
            .With(u => u.Email, testEmail)
            .Create();
    
        var token = _fixture.Build<PasswordResetToken>()
            .With(t => t.Email, testEmail)
            .With(t => t.Token, testToken)
            .With(t => t.ExpiryDate, DateTime.UtcNow.AddHours(1)) 
            .Create();
   
        var request = new ResetPasswordDto
        {
            Email = testEmail,
            Token = testToken,
            Password = newPassword,
            ConfirmPassword = confirmPassword
        };

        _tokenRepositoryMock.Setup(x => x.GetResetTokenAsync(testEmail, testToken))
            .ReturnsAsync(token);

        _userRepositoryMock.Setup(x => x.FindUserByEmailAsync(testEmail))
            .ReturnsAsync(testUser);

        // Act
        var result = await _controller.ResetPassword(request);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be(message);
    
        _userRepositoryMock.Verify(x => x.UpdateUserAsync(testUser), Times.Never);
    }
}
