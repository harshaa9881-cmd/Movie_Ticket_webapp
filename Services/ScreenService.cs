using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository _repository;

    public ScreenService(IScreenRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ScreenResponse>> GetAllScreensAsync(
        int? theatreId)
    {
        var screens = await _repository.GetAllAsync(theatreId);

        return screens.Select(s => MapToResponse(s)).ToList();
    }

    public async Task<ScreenResponse?> GetScreenByIdAsync(int id)
    {
        var screen = await _repository.GetByIdAsync(id);

        if (screen is null)
            return null;

        return MapToResponse(screen);
    }

    public async Task<List<ScreenResponse>> GetScreensByTheatreIdAsync(
        int theatreId)
    {
        var screens = await _repository.GetAllAsync(theatreId);

        return screens.Select(s => MapToResponse(s)).ToList();
    }

    public async Task<ScreenResponse> AddScreenAsync(
        CreateScreenRequest request)
    {
        if (!await _repository.TheatreExistsAsync(request.TheatreId))
        {
            throw new KeyNotFoundException("Theatre not found.");
        }

        string screenName = request.ScreenName.Trim();

        if (await _repository.ScreenNameExistsAsync(
            request.TheatreId,
            screenName))
        {
            throw new InvalidOperationException(
                "A screen with this name already exists in the theatre.");
        }

        var screen = new Screen
        {
            TheatreId = request.TheatreId,
            ScreenName = screenName
        };

        await _repository.AddAsync(screen);

        var savedScreen = await _repository.GetByIdAsync(screen.ScreenId);

        return MapToResponse(savedScreen!);
    }

    public async Task<ScreenResponse?> UpdateScreenAsync(
        int id,
        UpdateScreenRequest request)
    {
        var screen = await _repository.GetByIdAsync(id);

        if (screen is null)
            return null;

        if (!await _repository.TheatreExistsAsync(request.TheatreId))
        {
            throw new KeyNotFoundException("Theatre not found.");
        }

        string screenName = request.ScreenName.Trim();

        if (await _repository.ScreenNameExistsAsync(
            request.TheatreId,
            screenName,
            id))
        {
            throw new InvalidOperationException(
                "A screen with this name already exists in the theatre.");
        }

        screen.TheatreId = request.TheatreId;
        screen.ScreenName = screenName;

        await _repository.UpdateAsync(screen);

        var updatedScreen = await _repository.GetByIdAsync(id);

        return MapToResponse(updatedScreen!);
    }

    public async Task<string> DeleteScreenAsync(int id)
    {
        var screen = await _repository.GetByIdAsync(id);

        if (screen is null)
            return "NotFound";

        if (await _repository.HasSeatsOrShowsAsync(id))
            return "Conflict";

        await _repository.DeleteAsync(screen);

        return "Deleted";
    }

    private static ScreenResponse MapToResponse(Screen screen)
    {
        return new ScreenResponse
        {
            ScreenId = screen.ScreenId,
            TheatreId = screen.TheatreId,
            TheatreName = screen.Theatre.Name,
            ScreenName = screen.ScreenName
        };
    }
}