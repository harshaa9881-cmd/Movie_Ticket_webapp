using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IBookingRepository
{
    Task<bool> ShowExistsAsync(int showId);
    Task<Booking> AddBookingAsync(Booking booking, List<int> seatIds);
}