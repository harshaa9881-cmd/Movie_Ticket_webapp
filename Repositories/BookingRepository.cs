using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class BookingRepository : IBookingRepository
{
    private readonly ApplicationDbContext _context;

    public BookingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ShowExistsAsync(int showId)
    {
        return await _context.Shows.AnyAsync(s => s.ShowId == showId);
    }

    public async Task<Booking> AddBookingAsync(Booking booking, List<int> seatIds)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var show = await _context.Shows
            .FirstOrDefaultAsync(s => s.ShowId == booking.ShowId);

        if (show == null)
            throw new KeyNotFoundException("Show not found.");

        var showSeats = await _context.ShowSeats
            .Include(s => s.Seat)
            .Where(s => s.ShowId == booking.ShowId && seatIds.Contains(s.SeatId))
            .ToListAsync();

        if (showSeats.Count != seatIds.Count)
            throw new ArgumentException("One or more seats do not belong to this show.");

        if (showSeats.Any(s => s.Status != "Available"))
            throw new InvalidOperationException("One or more seats are no longer available.");

        booking.TotalAmount = show.TicketPrice * showSeats.Count;

        booking.BookingSeats = showSeats.Select(s => new BookingSeat
        {
            SeatId = s.SeatId,
            Price = show.TicketPrice,
            Seat = s.Seat
        }).ToList();

        foreach (var showSeat in showSeats)
        {
            showSeat.Status = "Booked";
        }

        _context.Bookings.Add(booking);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return booking;
    }
    public async Task<List<Booking>> GetAllAsync(int? showId, string? status)
    {
        var query = _context.Bookings
            .Include(b => b.BookingSeats)
            .ThenInclude(bs => bs.Seat)
            .AsQueryable();

        if (showId.HasValue)
            query = query.Where(b => b.ShowId == showId.Value);

        if (!string.IsNullOrEmpty(status))
            query = query.Where(b => b.Status == status);

        return await query
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    public async Task<Booking?> GetByIdAsync(int bookingId)
    {
        return await _context.Bookings
            .Include(b => b.BookingSeats)
            .ThenInclude(bs => bs.Seat)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);
    }

    public async Task<List<Booking>> GetByShowIdAsync(int showId)
    {
        return await _context.Bookings
            .Include(b => b.BookingSeats)
            .ThenInclude(bs => bs.Seat)
            .Where(b => b.ShowId == showId)
            .OrderByDescending(b => b.BookingDate)
            .ToListAsync();
    }

    public async Task<Booking> CancelBookingAsync(int bookingId)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var booking = await _context.Bookings
            .Include(b => b.BookingSeats)
            .FirstOrDefaultAsync(b => b.BookingId == bookingId);

        if (booking == null)
            throw new KeyNotFoundException("Booking not found.");

        if (booking.Status != "Confirmed")
            throw new InvalidOperationException(
                "Only confirmed bookings can be cancelled.");

        booking.Status = "Cancelled";

        var seatIds = booking.BookingSeats
            .Select(bs => bs.SeatId)
            .ToList();

        var showSeats = await _context.ShowSeats
            .Where(s => s.ShowId == booking.ShowId &&
                        seatIds.Contains(s.SeatId))
            .ToListAsync();

        foreach (var showSeat in showSeats)
        {
            showSeat.Status = "Available";
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return await GetByIdAsync(bookingId)
            ?? throw new KeyNotFoundException("Booking not found.");
    }
}