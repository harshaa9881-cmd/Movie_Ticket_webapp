using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;

    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public async Task<BookingResponse> CreateBookingAsync(CreateBookingRequest request)
    {
        if (!await _bookingRepository.ShowExistsAsync(request.ShowId))
            throw new KeyNotFoundException("Show not found.");

        if (request.SeatIds == null || request.SeatIds.Count == 0)
            throw new ArgumentException("At least one seat must be selected.");

        if (request.SeatIds.Distinct().Count() != request.SeatIds.Count)
            throw new ArgumentException("Duplicate seat IDs are not allowed.");

        var booking = new Booking
        {
            BookingReference = "MOV" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper(),
            ShowId = request.ShowId,
            CustomerName = request.CustomerName,
            CustomerEmail = request.CustomerEmail,
            CustomerPhone = request.CustomerPhone,
            BookingDate = DateTime.UtcNow,
            Status = "Confirmed"
        };

        var createdBooking = await _bookingRepository.AddBookingAsync(
            booking, request.SeatIds);

        return MapToResponse(createdBooking);
    }
    public async Task<List<BookingResponse>> GetAllAsync(
    int? showId, string? status)
    {
        var bookings = await _bookingRepository.GetAllAsync(showId, status);

        return bookings.Select(MapToResponse).ToList();
    }

    public async Task<BookingResponse?> GetByIdAsync(int bookingId)
    {
        var booking = await _bookingRepository.GetByIdAsync(bookingId);

        if (booking == null)
            return null;

        return MapToResponse(booking);
    }

    public async Task<List<BookingResponse>> GetByShowIdAsync(int showId)
    {
        if (!await _bookingRepository.ShowExistsAsync(showId))
            throw new KeyNotFoundException("Show not found.");

        var bookings = await _bookingRepository.GetByShowIdAsync(showId);

        return bookings.Select(MapToResponse).ToList();
    }

    public async Task<BookingResponse> CancelBookingAsync(int bookingId)
    {
        var booking = await _bookingRepository.CancelBookingAsync(bookingId);

        return MapToResponse(booking);
    }

    private static BookingResponse MapToResponse(Booking booking)
    {
        return new BookingResponse
        {
            BookingId = booking.BookingId,
            BookingReference = booking.BookingReference,
            ShowId = booking.ShowId,
            CustomerName = booking.CustomerName,
            CustomerEmail = booking.CustomerEmail,
            CustomerPhone = booking.CustomerPhone,
            BookingDate = booking.BookingDate,
            TotalAmount = booking.TotalAmount,
            Status = booking.Status,
            Seats = booking.BookingSeats
                .Select(s => s.Seat.RowName + s.Seat.SeatNumber)
                .ToList()
        };
    }
}