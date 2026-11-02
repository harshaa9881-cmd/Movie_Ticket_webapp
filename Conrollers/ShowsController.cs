using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ShowsController : ControllerBase
{
    private readonly IShowService _showService;

    public ShowsController(IShowService showService)
    {
        _showService = showService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllShows(
        [FromQuery] DateTime? date,
        [FromQuery] int? movieId,
        [FromQuery] int? theatreId)
    {
        if (movieId.HasValue && movieId <= 0)
            return BadRequest("Invalid movie ID.");

        if (theatreId.HasValue && theatreId <= 0)
            return BadRequest("Invalid theatre ID.");

        var shows = await _showService.GetAllShowsAsync(date, movieId, theatreId);

        return Ok(shows);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShowById(int id)
    {
        var show = await _showService.GetShowByIdAsync(id);

        if (show == null)
            return NotFound();

        return Ok(show);
    }

    [HttpGet("movie/{movieId}")]
    public async Task<IActionResult> GetShowsByMovieId(int movieId)
    {
        if (movieId <= 0)
            return BadRequest("Invalid movie ID.");

        return Ok(await _showService.GetShowsByMovieIdAsync(movieId));
    }

    [HttpGet("theatre/{theatreId}")]
    public async Task<IActionResult> GetShowsByTheatreId(int theatreId)
    {
        if (theatreId <= 0)
            return BadRequest("Invalid theatre ID.");

        return Ok(await _showService.GetShowsByTheatreIdAsync(theatreId));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchShowsByDate([FromQuery] DateTime date)
    {
        if (date == default)
            return BadRequest("A valid date is required.");

        return Ok(await _showService.SearchShowsByDateAsync(date));
    }

    [HttpPost]
    public async Task<IActionResult> AddShow(CreateShowRequest request)
    {
        try
        {
            var show = await _showService.AddShowAsync(request);

            return CreatedAtAction(nameof(GetShowById),
                new { id = show.ShowId }, show);
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
    public async Task<IActionResult> UpdateShow(int id, UpdateShowRequest request)
    {
        try
        {
            var show = await _showService.UpdateShowAsync(id, request);

            if (show == null)
                return NotFound();

            return Ok(show);
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
    public async Task<IActionResult> DeleteShow(int id)
    {
        var result = await _showService.DeleteShowAsync(id);

        if (result == "NotFound")
            return NotFound();

        if (result == "Conflict")
            return Conflict("Show cannot be deleted because dependent records exist.");

        return NoContent();
    }
}