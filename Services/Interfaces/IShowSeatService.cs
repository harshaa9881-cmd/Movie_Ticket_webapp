using MovieTicketBooking.DTOs.ShowSeats;

namespace MovieTicketBooking.Services.Interfaces;

public interface IShowSeatService
{
    Task<List<ShowSeatResponse>> GetByShowIdAsync(int showId);
    Task<List<ShowSeatResponse>> GetAvailableSeatsAsync(int showId);
    Task<List<ShowSeatResponse>> GetBookedSeatsAsync(int showId);
}