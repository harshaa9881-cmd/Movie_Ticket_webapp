using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Constants;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/bookings")]

public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(CreateBookingRequest request)
    {
        try
        {
            var booking = await _bookingService.CreateBookingAsync(request);

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
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetById(int id)
    {
        var booking = await _bookingService.GetByIdAsync(id);

        if (booking == null)
            return NotFound();

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
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> CancelBooking(int id)
    {
        try
        {
            var booking = await _bookingService.CancelBookingAsync(id);

            return Ok(booking);
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