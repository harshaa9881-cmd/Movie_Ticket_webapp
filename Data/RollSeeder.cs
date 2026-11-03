using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Constants;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Data;

public static class RoleSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        var passwordHasher = scope.ServiceProvider
            .GetRequiredService<IPasswordHasher<User>>();

        var adminPassword = configuration["SeedUsers:AdminPassword"];

        if (string.IsNullOrWhiteSpace(adminPassword))
        {
            throw new InvalidOperationException(
                "Admin password configuration is missing.");
        }

        var existingAdmin = await context.Users
            .FirstOrDefaultAsync(x => x.Email == "admin@moviebooking.com");

        if (existingAdmin is not null)
        {
            return;
        }

        var admin = new User
        {
            FullName = "System Admin",
            Email = "admin@moviebooking.com",
            Role = UserRoles.Admin
        };

        admin.PasswordHash = passwordHasher.HashPassword(
            admin,
            adminPassword);

        context.Users.Add(admin);

        await context.SaveChangesAsync();
    }
}