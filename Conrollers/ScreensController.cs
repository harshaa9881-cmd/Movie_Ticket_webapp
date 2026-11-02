using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Constants;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/screens")]

public class ScreensController : ControllerBase
{
    private readonly IScreenService _screenService;

    public ScreensController(IScreenService screenService)
    {
        _screenService = screenService;
    }

    // GET: api/screens
    // GET: api/screens?theatreId=1
    [HttpGet]
    public async Task<IActionResult> GetAllScreens(
        [FromQuery] int? theatreId)
    {
        if (theatreId.HasValue && theatreId.Value <= 0)
            return BadRequest("Theatre ID must be greater than zero.");

        var screens = await _screenService.GetAllScreensAsync(theatreId);

        return Ok(screens);
    }

    // GET: api/screens/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetScreenById(int id)
    {
        var screen = await _screenService.GetScreenByIdAsync(id);

        if (screen is null)
            return NotFound(new { message = "Screen not found." });

        return Ok(screen);
    }

    // GET: api/screens/theatre/1
    [HttpGet("theatre/{theatreId:int}")]
    public async Task<IActionResult> GetScreensByTheatreId(int theatreId)
    {
        if (theatreId <= 0)
            return BadRequest("Theatre ID must be greater than zero.");

        var screens = await _screenService.GetScreensByTheatreIdAsync(
            theatreId);

        return Ok(screens);
    }

    // POST: api/screens
    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> AddScreen(
        [FromBody] CreateScreenRequest request)
    {
        try
        {
            var screen = await _screenService.AddScreenAsync(request);

            return CreatedAtAction(
                nameof(GetScreenById),
                new { id = screen.ScreenId },
                screen);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT: api/screens/1
    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UpdateScreen(
        int id,
        [FromBody] UpdateScreenRequest request)
    {
        try
        {
            var screen = await _screenService.UpdateScreenAsync(id, request);

            if (screen is null)
                return NotFound(new { message = "Screen not found." });

            return Ok(screen);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // DELETE: api/screens/1
    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> DeleteScreen(int id)
    {
        var result = await _screenService.DeleteScreenAsync(id);

        if (result == "NotFound")
            return NotFound(new { message = "Screen not found." });

        if (result == "Conflict")
            return Conflict(new
            {
                message = "Cannot delete a screen that has seats or shows."
            });

        return NoContent();
    }
}