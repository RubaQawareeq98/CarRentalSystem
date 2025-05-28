using CarRentalSystem.Api.Mappers.Authentication;
using CarRentalSystem.Api.Mappers.Users;
using CarRentalSystem.Api.Models.Authentication;
using CarRentalSystem.Api.Models.Profile;
using CarRentalSystem.Api.Services.Interfaces;
using CarRentalSystem.Db.Models;
using CarRentalSystem.Db.Repositories.Interfaces;
using Sieve.Models;

namespace CarRentalSystem.Api.Services;

public class UserService(IUserRepository userRepository,
    SignupRequestMapper mapToUser,
    UserProfileMapper profileMapper) : IUserService
{
    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await userRepository.FindUserByEmailAsync(email);
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await userRepository.FindUserByIdAsync(userId);
    }

    public async Task<User?> AuthenticateUserAsync(string email, string password)
    {
        var user = await userRepository.FindUserByEmailAsync(email);
        if (user is null) return null;

        return BCrypt.Net.BCrypt.Verify(password, user.Password) ? user : null;
    }

    public async Task RegisterUserAsync(User user)
    {
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        await userRepository.AddUserAsync(user);
    }

    public async Task UpdateUserAsync(UpdateProfileBodyDto bodyDto)
    {
        var user = profileMapper.UpdateUser(bodyDto);
        bodyDto.Password = BCrypt.Net.BCrypt.HashPassword(bodyDto.Password);
        await userRepository.UpdateUserAsync(user);
    }

    public async Task<List<User>> GetAllUsersAsync(SieveModel sieveModel)
    {
        return await userRepository.GetAllUsersAsync(sieveModel);
    }

    public async Task<bool> UserExistsAsync(Guid id)
    {
        return await userRepository.IsEntityExist(id);
    }
    
    public async Task<(bool Success, string Message)> SignupAsync(SignupRequestBodyDto request)
    {
        if (request.Password != request.ConfirmPassword)
        {
            return (false, "Passwords do not match.");
        }

        var existingUser = await userRepository.FindUserByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            return (false, "Email is already taken.");
        }

        var user = mapToUser.ToUser(request);
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        await userRepository.AddUserAsync(user);
        return (true, "User created successfully.");
    }

}