using MovieTicketBooking.DTOs.Shows;

namespace MovieTicketBooking.Services.Interfaces;

public interface IShowService
{
    Task<List<ShowResponse>> GetAllShowsAsync(
        DateTime? date, int? movieId, int? theatreId);
    Task<ShowResponse?> GetShowByIdAsync(int showId);
    Task<List<ShowResponse>> GetShowsByMovieIdAsync(int movieId);
    Task<List<ShowResponse>> GetShowsByTheatreIdAsync(int theatreId);
    Task<List<ShowResponse>> SearchShowsByDateAsync(DateTime date);
    Task<ShowResponse> AddShowAsync(CreateShowRequest request);
    Task<ShowResponse?> UpdateShowAsync(int showId, UpdateShowRequest request);
    Task<string> DeleteShowAsync(int showId);
}