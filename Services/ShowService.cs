using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class ShowService : IShowService
{
    private readonly IShowRepository _showRepository;

    public ShowService(IShowRepository showRepository)
    {
        _showRepository = showRepository;
    }

    public async Task<List<ShowResponse>> GetAllShowsAsync(
        DateTime? date, int? movieId, int? theatreId)
    {
        var shows = await _showRepository.GetAllAsync(date, movieId, theatreId);

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<ShowResponse?> GetShowByIdAsync(int showId)
    {
        var show = await _showRepository.GetByIdAsync(showId);

        if (show == null)
            return null;

        return MapToResponse(show);
    }

    public async Task<List<ShowResponse>> GetShowsByMovieIdAsync(int movieId)
    {
        var shows = await _showRepository.GetByMovieIdAsync(movieId);

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<List<ShowResponse>> GetShowsByTheatreIdAsync(int theatreId)
    {
        var shows = await _showRepository.GetByTheatreIdAsync(theatreId);

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<List<ShowResponse>> SearchShowsByDateAsync(DateTime date)
    {
        var shows = await _showRepository.GetByDateAsync(date);

        return shows.Select(MapToResponse).ToList();
    }

    public async Task<ShowResponse> AddShowAsync(CreateShowRequest request)
    {
        if (request.EndTime <= request.StartTime)
            throw new InvalidOperationException("End time must be later than start time.");

        if (request.TicketPrice < 0)
            throw new InvalidOperationException("Ticket price cannot be negative.");

        if (!await _showRepository.MovieExistsAsync(request.MovieId))
            throw new KeyNotFoundException("Movie not found.");

        if (!await _showRepository.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen not found.");

        if (await _showRepository.HasOverlappingShowAsync(
            request.ScreenId, request.StartTime, request.EndTime))
            throw new InvalidOperationException("Another show is already scheduled during this time.");

        var show = new Show
        {
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TicketPrice = request.TicketPrice
        };

        await _showRepository.AddAsync(show);

        var savedShow = await _showRepository.GetByIdAsync(show.ShowId);

        return MapToResponse(savedShow!);
    }

    public async Task<ShowResponse?> UpdateShowAsync(
        int showId, UpdateShowRequest request)
    {
        var show = await _showRepository.GetByIdAsync(showId);

        if (show == null)
            return null;

        if (request.EndTime <= request.StartTime)
            throw new InvalidOperationException("End time must be later than start time.");

        if (request.TicketPrice < 0)
            throw new InvalidOperationException("Ticket price cannot be negative.");

        if (!await _showRepository.MovieExistsAsync(request.MovieId))
            throw new KeyNotFoundException("Movie not found.");

        if (!await _showRepository.ScreenExistsAsync(request.ScreenId))
            throw new KeyNotFoundException("Screen not found.");

        if (await _showRepository.HasOverlappingShowAsync(
            request.ScreenId, request.StartTime, request.EndTime, showId))
            throw new InvalidOperationException("Another show is already scheduled during this time.");

        show.MovieId = request.MovieId;
        show.ScreenId = request.ScreenId;
        show.StartTime = request.StartTime;
        show.EndTime = request.EndTime;
        show.TicketPrice = request.TicketPrice;

        await _showRepository.UpdateAsync(show);

        var updatedShow = await _showRepository.GetByIdAsync(showId);

        return MapToResponse(updatedShow!);
    }

    public async Task<string> DeleteShowAsync(int showId)
    {
        var show = await _showRepository.GetByIdAsync(showId);

        if (show == null)
            return "NotFound";

        if (await _showRepository.HasDependenciesAsync(showId))
            return "Conflict";

        await _showRepository.DeleteAsync(show);

        return "Deleted";
    }

    private static ShowResponse MapToResponse(Show show)
    {
        return new ShowResponse
        {
            ShowId = show.ShowId,
            MovieId = show.MovieId,
            MovieTitle = show.Movie.Title,
            ScreenId = show.ScreenId,
            ScreenName = show.Screen.ScreenName,
            TheatreName = show.Screen.Theatre.Name,
            StartTime = show.StartTime,
            EndTime = show.EndTime,
            TicketPrice = show.TicketPrice
        };
    }
}