using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SeatsController : ControllerBase
{
    private readonly ISeatService _seatService;

    public SeatsController(ISeatService seatService)
    {
        _seatService = seatService;
    }

    [HttpGet("screen/{screenId}")]
    public async Task<IActionResult> GetSeatsByScreenId(int screenId)
    {
        if (screenId <= 0)
            return BadRequest("Invalid screen ID.");

        return Ok(await _seatService.GetSeatsByScreenIdAsync(screenId));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSeatById(int id)
    {
        var seat = await _seatService.GetSeatByIdAsync(id);

        if (seat == null)
            return NotFound();

        return Ok(seat);
    }

    [HttpPost]
    public async Task<IActionResult> AddSeat(CreateSeatRequest request)
    {
        try
        {
            var seat = await _seatService.AddSeatAsync(request);

            return CreatedAtAction(nameof(GetSeatById),
                new { id = seat.SeatId }, seat);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPost("bulk")]
    public async Task<IActionResult> AddBulkSeats(BulkCreateSeatsRequest request)
    {
        try
        {
            var seats = await _seatService.AddBulkSeatsAsync(request);

            return StatusCode(StatusCodes.Status201Created, seats);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSeat(int id, UpdateSeatRequest request)
    {
        try
        {
            var seat = await _seatService.UpdateSeatAsync(id, request);

            if (seat == null)
                return NotFound();

            return Ok(seat);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSeat(int id)
    {
        var result = await _seatService.DeleteSeatAsync(id);

        if (result == "NotFound")
            return NotFound();

        if (result == "Conflict")
            return Conflict("Seat cannot be deleted because show or booking records exist.");

        return NoContent();
    }
}