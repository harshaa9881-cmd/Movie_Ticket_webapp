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

        return new BookingResponse
        {
            BookingId = createdBooking.BookingId,
            BookingReference = createdBooking.BookingReference,
            ShowId = createdBooking.ShowId,
            TotalAmount = createdBooking.TotalAmount,
            Status = createdBooking.Status,
            Seats = createdBooking.BookingSeats
                .Select(s => s.Seat.RowName + s.Seat.SeatNumber)
                .ToList()
        };
    }
}