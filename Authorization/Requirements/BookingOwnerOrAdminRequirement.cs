using Microsoft.AspNetCore.Authorization;

namespace MovieTicketBooking.Authorization.Requirements;

public class BookingOwnerOrAdminRequirement : IAuthorizationRequirement
{
}