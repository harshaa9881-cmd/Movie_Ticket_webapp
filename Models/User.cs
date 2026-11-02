using System.ComponentModel.DataAnnotations;
using MovieTicketBooking.Constants;

namespace MovieTicketBooking.Models;

public class User
{
    public int UserId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string Role { get; set; } = UserRoles.Customer;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}