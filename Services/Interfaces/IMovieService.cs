using MovieTicketBooking.DTOs;
using MovieTicketBooking.DTOs.Movies;

namespace MovieTicketBooking.Services.Interfaces;

public interface IMovieService
{
    Task<PagedResult<MovieResponse>> GetAllMoviesAsync(
        string? title,
        int pageNumber,
        int pageSize);

    Task<MovieResponse?> GetMovieByIdAsync(int id);

    Task<MovieResponse> AddMovieAsync(CreateMovieRequest request);

    Task<MovieResponse?> UpdateMovieAsync(
        int id,
        UpdateMovieRequest request);

    Task<string> DeleteMovieAsync(int id);

    Task<List<MovieResponse>> SearchMoviesByTitleAsync(string title);
}