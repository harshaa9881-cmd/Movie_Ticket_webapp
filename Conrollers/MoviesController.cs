using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs;
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Services.Interfaces;
using MovieTicketBooking.Constants;

namespace MovieTicketBooking.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    // GET: api/movies
    [HttpGet]
    public async Task<ActionResult<PagedResult<MovieResponse>>> GetAllMovies(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? title = null)
    {
        if (pageNumber < 1)
            return BadRequest("Page number must be at least 1.");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("Page size must be between 1 and 100.");

        var movies = await _movieService.GetAllMoviesAsync(
            title,
            pageNumber,
            pageSize);

        return Ok(movies);
    }

    // GET: api/movies/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieResponse>> GetMovieById(int id)
    {
        if (id <= 0)
            return BadRequest("Movie ID must be greater than zero.");

        var movie = await _movieService.GetMovieByIdAsync(id);

        if (movie is null)
            return NotFound(new { message = "Movie not found." });

        return Ok(movie);
    }

    // POST: api/movies
    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<MovieResponse>> AddMovie(
        CreateMovieRequest request)
    {
        var movie = await _movieService.AddMovieAsync(request);

        return CreatedAtAction(
            nameof(GetMovieById),
            new { id = movie.MovieId },
            movie);
    }

    // PUT: api/movies/1
    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<ActionResult<MovieResponse>> UpdateMovie(
        int id,
        UpdateMovieRequest request)
    {
        if (id <= 0)
            return BadRequest("Movie ID must be greater than zero.");

        var movie = await _movieService.UpdateMovieAsync(id, request);

        if (movie is null)
            return NotFound(new { message = "Movie not found." });

        return Ok(movie);
    }

    // DELETE: api/movies/1
    [HttpDelete("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        if (id <= 0)
            return BadRequest("Movie ID must be greater than zero.");

        var result = await _movieService.DeleteMovieAsync(id);

        if (result == "NotFound")
            return NotFound(new { message = "Movie not found." });

        if (result == "Conflict")
            return Conflict(new
            {
                message = "Movie cannot be deleted because shows are associated with it."
            });

        return NoContent();
    }

    // GET: api/movies/search?title=leo
    [HttpGet("search")]
    public async Task<ActionResult<List<MovieResponse>>> SearchMovies(
        [FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Search title is required.");

        var movies = await _movieService.SearchMoviesByTitleAsync(title);

        return Ok(movies);
    }
}