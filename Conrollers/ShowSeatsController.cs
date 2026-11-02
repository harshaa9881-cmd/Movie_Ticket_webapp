using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Constants;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/shows/{showId}/seats")]
[Authorize]
public class ShowSeatsController : ControllerBase
{
    private readonly IShowSeatService _showSeatService;

    public ShowSeatsController(IShowSeatService showSeatService)
    {
        _showSeatService = showSeatService;
    }

    [HttpGet]
    public async Task<IActionResult> GetByShowId(int showId)
    {
        try
        {
            return Ok(await _showSeatService.GetByShowIdAsync(showId));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableSeats(int showId)
    {
        try
        {
            return Ok(await _showSeatService.GetAvailableSeatsAsync(showId));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("booked")]
    public async Task<IActionResult> GetBookedSeats(int showId)
    {
        try
        {
            return Ok(await _showSeatService.GetBookedSeatsAsync(showId));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}