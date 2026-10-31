using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/theatres")]
public class TheatresController : ControllerBase
{
    private readonly ITheatreService _theatreService;

    public TheatresController(ITheatreService theatreService)
    {
        _theatreService = theatreService;
    }

    // GET: api/theatres
    [HttpGet]
    public async Task<IActionResult> GetAllTheatres()
    {
        var theatres = await _theatreService.GetAllTheatresAsync();

        return Ok(theatres);
    }

    // GET: api/theatres/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTheatreById(int id)
    {
        var theatre = await _theatreService.GetTheatreByIdAsync(id);

        if (theatre is null)
            return NotFound(new { message = "Theatre not found." });

        return Ok(theatre);
    }

    // POST: api/theatres
    [HttpPost]
    public async Task<IActionResult> AddTheatre(
        [FromBody] CreateTheatreRequest request)
    {
        var theatre = await _theatreService.AddTheatreAsync(request);

        return CreatedAtAction(
            nameof(GetTheatreById),
            new { id = theatre.TheatreId },
            theatre);
    }

    // PUT: api/theatres/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTheatre(
        int id,
        [FromBody] UpdateTheatreRequest request)
    {
        var theatre = await _theatreService.UpdateTheatreAsync(id, request);

        if (theatre is null)
            return NotFound(new { message = "Theatre not found." });

        return Ok(theatre);
    }

    // DELETE: api/theatres/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTheatre(int id)
    {
        var result = await _theatreService.DeleteTheatreAsync(id);

        if (result == "NotFound")
            return NotFound(new { message = "Theatre not found." });

        if (result == "Conflict")
            return Conflict(new
            {
                message = "Cannot delete a theatre that has screens."
            });

        return NoContent();
    }
}