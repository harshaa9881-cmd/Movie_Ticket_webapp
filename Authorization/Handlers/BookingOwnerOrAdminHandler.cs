using Microsoft.AspNetCore.Authorization;
using MovieTicketBooking.Constants;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Authorization.Requirements;
using System.Security.Claims;

namespace MovieTicketBooking.Authorization.Handlers;

public class BookingOwnerOrAdminHandler
    : AuthorizationHandler<BookingOwnerOrAdminRequirement, int>
{
    private readonly IBookingRepository _bookingRepository;

    public BookingOwnerOrAdminHandler(
        IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        BookingOwnerOrAdminRequirement requirement,
        int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        if (booking == null)
            return;

        // Admin can access any booking
        if (context.User.IsInRole(UserRoles.Admin))
        {
            context.Succeed(requirement);
            return;
        }

        // Get the logged-in user's ID from JWT
        var userIdValue = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return;

        // Customer can access only their own booking
        if (booking.UserId == userId)
        {
            context.Succeed(requirement);
        }
    }
}