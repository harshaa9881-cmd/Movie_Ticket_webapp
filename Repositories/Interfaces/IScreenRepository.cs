using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IScreenRepository
{
    Task<List<Screen>> GetAllAsync(int? theatreId);

    Task<Screen?> GetByIdAsync(int id);

    Task<bool> TheatreExistsAsync(int theatreId);

    Task<bool> ScreenNameExistsAsync(
        int theatreId,
        string screenName,
        int? excludeScreenId = null);

    Task AddAsync(Screen screen);

    Task UpdateAsync(Screen screen);

    Task DeleteAsync(Screen screen);

    Task<bool> HasSeatsOrShowsAsync(int screenId);
}