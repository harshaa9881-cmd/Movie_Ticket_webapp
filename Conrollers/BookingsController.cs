using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Constants;
using System.Security.Claims;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/bookings")]

public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IAuthorizationService _authorizationService;
    public BookingsController(IBookingService bookingService, IAuthorizationService authorizationService)
    {
        _bookingService = bookingService;
        _authorizationService = authorizationService;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Customer)]
    public async Task<IActionResult> CreateBooking(CreateBookingRequest request)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(userIdValue, out int userId))
            return Unauthorized();

        try
        {
            var booking = await _bookingService.CreateBookingAsync(
                request, userId);

            return Created($"/api/bookings/{booking.BookingId}", booking);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetAll(
    [FromQuery] int? showId,
    [FromQuery] string? status)
    {
        var bookings = await _bookingService.GetAllAsync(showId, status);

        return Ok(bookings);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Customer)]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);

        if (booking == null)
            return NotFound();

        var authorizationResult =
            await _authorizationService.AuthorizeAsync(
                User,
                id,
                "BookingOwnerOrAdmin");

        if (!authorizationResult.Succeeded)
            return Forbid();

        return Ok(booking);
    }

    [HttpGet("show/{showId}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetByShowId(int showId)
    {
        try
        {
            var bookings = await _bookingService.GetByShowIdAsync(showId);

            return Ok(bookings);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
    [HttpPut("{id}/cancel")]
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Customer)]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);

        if (booking == null)
            return NotFound();

        var authorizationResult =
            await _authorizationService.AuthorizeAsync(
                User,
                id,
                "BookingOwnerOrAdmin");

        if (!authorizationResult.Succeeded)
            return Forbid();

        try
        {
            var cancelledBooking =
                await _bookingService.CancelBookingAsync(id);

            return Ok(cancelledBooking);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}