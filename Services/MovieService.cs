using MovieTicketBooking.DTOs;
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;

    public MovieService(IMovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResult<MovieResponse>> GetAllMoviesAsync(
        string? title,
        int pageNumber,
        int pageSize)
    {
        var movies = await _repository.GetAllAsync(
            title,
            pageNumber,
            pageSize);

        var totalCount = await _repository.GetTotalCountAsync(title);

        var movieResponses = movies.Select(m => MapToResponse(m)).ToList();

        return new PagedResult<MovieResponse>
        {
            Items = movieResponses,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task<MovieResponse?> GetMovieByIdAsync(int id)
    {
        var movie = await _repository.GetByIdAsync(id);

        if (movie is null)
            return null;

        return MapToResponse(movie);
    }

    public async Task<MovieResponse> AddMovieAsync(
        CreateMovieRequest request)
    {
        var movie = new Movie
        {
            Title = request.Title,
            Description = request.Description,
            DurationMinutes = request.DurationMinutes,
            ReleaseDate = request.ReleaseDate,
            Language = request.Language,
            Genre = request.Genre
        };

        await _repository.AddAsync(movie);

        return MapToResponse(movie);
    }

    public async Task<MovieResponse?> UpdateMovieAsync(
        int id,
        UpdateMovieRequest request)
    {
        var movie = await _repository.GetByIdAsync(id);

        if (movie is null)
            return null;

        movie.Title = request.Title;
        movie.Description = request.Description;
        movie.DurationMinutes = request.DurationMinutes;
        movie.ReleaseDate = request.ReleaseDate;
        movie.Language = request.Language;
        movie.Genre = request.Genre;

        await _repository.UpdateAsync(movie);

        return MapToResponse(movie);
    }

    public async Task<string> DeleteMovieAsync(int id)
    {
        var movie = await _repository.GetByIdAsync(id);

        if (movie is null)
            return "NotFound";

        if (await _repository.HasShowsAsync(id))
            return "Conflict";

        await _repository.DeleteAsync(movie);

        return "Deleted";
    }

    public async Task<List<MovieResponse>> SearchMoviesByTitleAsync(
        string title)
    {
        var movies = await _repository.SearchByTitleAsync(title);

        return movies.Select(m => MapToResponse(m)).ToList();
    }

    private static MovieResponse MapToResponse(Movie movie)
    {
        return new MovieResponse
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Language = movie.Language,
            Genre = movie.Genre
        };
    }
}