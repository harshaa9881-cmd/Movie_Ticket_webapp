using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface ISeatRepository
{
    Task<List<Seat>> GetByScreenIdAsync(int screenId);
    Task<Seat?> GetByIdAsync(int seatId);
    Task<bool> ScreenExistsAsync(int screenId);
    Task<bool> SeatExistsAsync(int screenId, string rowName, int seatNumber, int? excludeSeatId = null);
    Task AddAsync(Seat seat);
    Task AddRangeAsync(List<Seat> seats);
    Task UpdateAsync(Seat seat);
    Task DeleteAsync(Seat seat);
    Task<bool> HasBookingsOrShowsAsync(int seatId);
}