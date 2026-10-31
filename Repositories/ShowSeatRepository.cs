using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class ShowSeatRepository : IShowSeatRepository
{
    private readonly ApplicationDbContext _context;

    public ShowSeatRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ShowExistsAsync(int showId)
    {
        return await _context.Shows.AnyAsync(s => s.ShowId == showId);
    }

    public async Task<List<ShowSeat>> GetByShowIdAsync(int showId)
    {
        return await _context.ShowSeats
            .Include(s => s.Seat)
            .Include(s => s.Show)
            .Where(s => s.ShowId == showId)
            .OrderBy(s => s.Seat.RowName)
            .ThenBy(s => s.Seat.SeatNumber)
            .ToListAsync();
    }

    public async Task<List<ShowSeat>> GetByStatusAsync(int showId, string status)
    {
        return await _context.ShowSeats
            .Include(s => s.Seat)
            .Include(s => s.Show)
            .Where(s => s.ShowId == showId && s.Status == status)
            .OrderBy(s => s.Seat.RowName)
            .ThenBy(s => s.Seat.SeatNumber)
            .ToListAsync();
    }
}