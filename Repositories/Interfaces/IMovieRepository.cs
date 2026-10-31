using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetAllAsync(
        string? title,
        int pageNumber,
        int pageSize);

    Task<int> GetTotalCountAsync(string? title);

    Task<Movie?> GetByIdAsync(int id);

    Task AddAsync(Movie movie);

    Task UpdateAsync(Movie movie);

    Task DeleteAsync(Movie movie);

    Task<List<Movie>> SearchByTitleAsync(string title);

    Task<bool> HasShowsAsync(int movieId);
}