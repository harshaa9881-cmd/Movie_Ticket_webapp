
using Microsoft.AspNetCore.Identity;
using MovieTicketBooking.Constants;
using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IJwtService _jwtService;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasher<User> passwordHasher,
        IJwtService jwtService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtService = jwtService;
    }

    public async Task<(bool Success, string Message)> RegisterAsync(
        RegisterDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var existingUser =
            await _userRepository.GetByEmailAsync(email);

        if (existingUser is not null)
        {
            return (false, "Email is already registered.");
        }

        var user = new User
        {
            FullName = dto.FullName,
            Email = email,
            PasswordHash = string.Empty,
            Role = UserRoles.Customer
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            dto.Password
        );

        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return (true, "Registration successful.");
    }

    public async Task<(bool Success, string Message, string? Token)> LoginAsync(
        LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();

        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            return (false, "Invalid email or password.", null);
        }

        var result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            dto.Password
        );

        if (result == PasswordVerificationResult.Failed)
        {
            return (false, "Invalid email or password.", null);
        }

        var token = _jwtService.GenerateToken(user);

        return (true, "Login successful.", token);
    }
}
