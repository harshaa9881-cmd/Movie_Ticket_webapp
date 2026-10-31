using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<bool> ShowExistsAsync(int showId);
    Task<Booking> AddBookingAsync(Booking booking, List<int> seatIds);

    Task<List<Booking>> GetAllAsync(int? showId, string? status);
    Task<Booking?> GetByIdAsync(int bookingId);
    Task<List<Booking>> GetByShowIdAsync(int showId);
    Task<Booking> CancelBookingAsync(int bookingId);
}