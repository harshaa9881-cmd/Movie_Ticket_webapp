using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories;

public class ShowRepository : IShowRepository
{
    private readonly ApplicationDbContext _context;

    public ShowRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Show>> GetAllAsync(
        DateTime? date, int? movieId, int? theatreId)
    {
        var query = _context.Shows
            .Include(s => s.Movie)
            .Include(s => s.Screen)
                .ThenInclude(s => s.Theatre)
            .AsQueryable();

        if (date.HasValue)
            query = query.Where(s => s.StartTime.Date == date.Value.Date);

        if (movieId.HasValue)
            query = query.Where(s => s.MovieId == movieId.Value);

        if (theatreId.HasValue)
            query = query.Where(s => s.Screen.TheatreId == theatreId.Value);

        return await query.OrderBy(s => s.StartTime).ToListAsync();
    }

    public async Task<Show?> GetByIdAsync(int showId)
    {
        return await _context.Shows
            .Include(s => s.Movie)
            .Include(s => s.Screen)
                .ThenInclude(s => s.Theatre)
            .FirstOrDefaultAsync(s => s.ShowId == showId);
    }

    public async Task<List<Show>> GetByMovieIdAsync(int movieId)
    {
        return await _context.Shows
            .Include(s => s.Movie)
            .Include(s => s.Screen)
                .ThenInclude(s => s.Theatre)
            .Where(s => s.MovieId == movieId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<List<Show>> GetByTheatreIdAsync(int theatreId)
    {
        return await _context.Shows
            .Include(s => s.Movie)
            .Include(s => s.Screen)
                .ThenInclude(s => s.Theatre)
            .Where(s => s.Screen.TheatreId == theatreId)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<List<Show>> GetByDateAsync(DateTime date)
    {
        return await _context.Shows
            .Include(s => s.Movie)
            .Include(s => s.Screen)
                .ThenInclude(s => s.Theatre)
            .Where(s => s.StartTime.Date == date.Date)
            .OrderBy(s => s.StartTime)
            .ToListAsync();
    }

    public async Task<bool> MovieExistsAsync(int movieId)
    {
        return await _context.Movies.AnyAsync(m => m.MovieId == movieId);
    }

    public async Task<bool> ScreenExistsAsync(int screenId)
    {
        return await _context.Screens.AnyAsync(s => s.ScreenId == screenId);
    }

    public async Task<bool> HasOverlappingShowAsync(
        int screenId, DateTime startTime, DateTime endTime, int? excludeShowId = null)
    {
        return await _context.Shows.AnyAsync(s =>
            s.ScreenId == screenId &&
            s.ShowId != excludeShowId &&
            startTime < s.EndTime &&
            endTime > s.StartTime);
    }

    public async Task<bool> HasDependenciesAsync(int showId)
    {
        return await _context.ShowSeats.AnyAsync(s => s.ShowId == showId) ||
               await _context.Bookings.AnyAsync(b => b.ShowId == showId);
    }

    public async Task AddAsync(Show show)
    {
        await _context.Shows.AddAsync(show);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Show show)
    {
        _context.Shows.Update(show);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Show show)
    {
        _context.Shows.Remove(show);
        await _context.SaveChangesAsync();
    }
    public async Task<List<Seat>> GetSeatsByScreenIdAsync(int screenId)
    {
        return await _context.Seats
            .Where(s => s.ScreenId == screenId)
            .OrderBy(s => s.RowName)
            .ThenBy(s => s.SeatNumber)
            .ToListAsync();
    }
}