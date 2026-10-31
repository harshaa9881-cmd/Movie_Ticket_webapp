using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IShowSeatRepository
{
    Task<bool> ShowExistsAsync(int showId);
    Task<List<ShowSeat>> GetByShowIdAsync(int showId);
    Task<List<ShowSeat>> GetByStatusAsync(int showId, string status);
}