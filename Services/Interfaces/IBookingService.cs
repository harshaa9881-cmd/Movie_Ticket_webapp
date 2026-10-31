using MovieTicketBooking.DTOs.Bookings;

namespace MovieTicketBooking.Services.Interfaces;

public interface IBookingService
{
    Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request);
    Task<List<BookingResponse>> GetAllAsync(int? showId, string? status);
    Task<BookingResponse?> GetByIdAsync(int bookingId);
    Task<List<BookingResponse>> GetByShowIdAsync(int showId);
    Task<BookingResponse> CancelBookingAsync(int bookingId);
}