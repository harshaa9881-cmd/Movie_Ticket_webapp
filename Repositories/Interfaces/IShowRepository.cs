using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IShowRepository
{
    Task<List<Show>> GetAllAsync(DateTime? date, int? movieId, int? theatreId);
    Task<Show?> GetByIdAsync(int showId);
    Task<List<Show>> GetByMovieIdAsync(int movieId);
    Task<List<Show>> GetByTheatreIdAsync(int theatreId);
    Task<List<Show>> GetByDateAsync(DateTime date);
    Task<bool> MovieExistsAsync(int movieId);
    Task<bool> ScreenExistsAsync(int screenId);
    Task<bool> HasOverlappingShowAsync(
        int screenId, DateTime startTime, DateTime endTime, int? excludeShowId = null);
    Task<bool> HasDependenciesAsync(int showId);
    Task AddAsync(Show show);
    Task UpdateAsync(Show show);
    Task DeleteAsync(Show show);
}